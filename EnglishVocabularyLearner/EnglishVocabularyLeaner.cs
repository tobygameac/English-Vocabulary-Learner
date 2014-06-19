using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace EnglishVocabularyLearner {

  public partial class EnglishVocabularyLeaner : Form {
    private List<Vocabulary> vocList = new List<Vocabulary>();

    // Vocabulary Pool, in order to improve the speed of getting new question
    private List<Vocabulary> vocabularyPool;

    // Choose index from the list
    NumberChooser numberChooser = new NumberChooser();

    // Variables for test
    private int testType;
    private int questionNumbers;
    private int nowQuestionNumber;
    private int lastQuestionNumber;
    private int nowWrongNumber; // Test type 2
    private bool isTesting;
    private bool isTestFinished;

    // Record status of every question
    private List<Vocabulary> questions;
    private List<String> answers;
    private List<bool> answerStatus;

    // Speech
    private SpeechRecognitionEngine enSRE;
    private SpeechRecognitionEngine chSRE;
    private SpeechSynthesizer enSS;
    private bool isChCommand;

    public EnglishVocabularyLeaner() {
      InitializeComponent();

      //GetVocabularyList();

      try { // Read list.txt in the same folder as default
        FileStream fileStream = new FileStream("list.txt", FileMode.Open, FileAccess.Read);
        this.labelFileReadStatus.Text = "從預設目錄中讀取檔案成功";
        inputStreamParser(fileStream);
        fileStream.Close();
      } catch (FileNotFoundException ioEx) {
        Console.Write(ioEx);
        this.labelFileReadStatus.Text = "預設讀取失敗，請手動加入檔案";
      }

      // Add events
      this.buttonFileRead.Click += new EventHandler(this.readByCustomFile);
      this.buttonStartTest1.Click += new EventHandler(this.readyToStartTest);
      this.buttonStartTest2.Click += new EventHandler(this.readyToStartTest);
      this.buttonNextQuestion.Click += new EventHandler(this.nextQuestion);
      this.buttonPrevQuestion.Click += new EventHandler(this.prevQuestion);
      this.buttonFinishTest.Click += new EventHandler(this.readyToCloseTest);

      this.textBoxVocabulary.KeyDown += new KeyEventHandler(this.submitVocabulary);
      this.textBoxAnswer.KeyDown += new KeyEventHandler(this.submitAnswer);
      this.textBoxAnswer.VisibleChanged += new EventHandler(this.textBoxAutoFocus);

      // Speech
      speechRecognitionEngineInitial();
      speechSynthesizerInitial();
    }

    private void EnglishVocabularyLeaner_Load(object sender, EventArgs e) {
      this.DoubleBuffered = true;
      Form.CheckForIllegalCrossThreadCalls = false;
      isTesting = false;
    }

    private void speechRecognitionEngineInitial() {
      enSRE = new SpeechRecognitionEngine(new CultureInfo("en-US"));
      /*
      List<List<string>> alphabetsCombination = new List<List<string>>();
      alphabetsCombination.Add(new List<string>());
      alphabetsCombination[0].Add("");
      for (int len = 1; len <= 0; len++) {
        alphabetsCombination.Add(new List<string>());
        foreach (string alphabetsString in alphabetsCombination[len - 1]) {
          for (int i = 0; i < 26; i++) {
            alphabetsCombination[len].Add(alphabetsString + ((char)('a' + i)).ToString());
          }
        }
      }
      Choices enCommandChoices = new Choices();
      for (int len = 1; len < alphabetsCombination.Count; len++) {
        foreach (string alphabetsString in alphabetsCombination[len]) {
          enCommandChoices.Add(alphabetsString);
        }
      }
      enCommandChoices.Add("go");
      enCommandChoices.Add("enter");
      enCommandChoices.Add("done");
      enCommandChoices.Add("finish");
      enCommandChoices.Add("next");
      enCommandChoices.Add("clear");*/
      Choices enCommandChoices = new Choices();
      for (int i = 0; i < vocList.Count; i++) {
        enCommandChoices.Add(vocList[i].text);
      }
      GrammarBuilder enCommandGrammarBuilder = new GrammarBuilder(enCommandChoices);

      enSRE.LoadGrammar(new Grammar(enCommandGrammarBuilder));
      //enSRE.LoadGrammar(new DictationGrammar("grammar:dictation#spelling"));
      //enSRE.LoadGrammar(new DictationGrammar());

      enSRE.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(enSRESpeechDetected);
      enSRE.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(enSRESpeechRecognized);
      enSRE.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(enSRESpeechRecognitionRejected);
      enSRE.RecognizeCompleted += new EventHandler<RecognizeCompletedEventArgs>(enSRESpeechRecognizeCompleted);
      //enSRE.SetInputToDefaultAudioDevice();
      //enSRE.RecognizeAsync(RecognizeMode.Multiple);


      chSRE = new SpeechRecognitionEngine(new CultureInfo("zh-TW"));

      Choices chCommandChoices = new Choices();

      for (int i = 0; i < 26; i++) {
        chCommandChoices.Add(((char)('a' + i)).ToString());
      }

      chCommandChoices.Add("開始");
      chCommandChoices.Add("開始測驗一");
      chCommandChoices.Add("開始測驗二");

      chCommandChoices.Add("輸入");
      chCommandChoices.Add("送出");
      chCommandChoices.Add("完成");

      chCommandChoices.Add("清除");
      chCommandChoices.Add("刪除");

      chCommandChoices.Add("往前");
      chCommandChoices.Add("往後");
      chCommandChoices.Add("往上");
      chCommandChoices.Add("往下");
      chCommandChoices.Add("上一題");
      chCommandChoices.Add("下一題");
      chCommandChoices.Add("繼續");

      chCommandChoices.Add("關閉");
      chCommandChoices.Add("結束");

      GrammarBuilder chCommandGrammarBuilder = new GrammarBuilder(chCommandChoices);
      chCommandGrammarBuilder.Culture = new CultureInfo("zh-TW");

      chSRE.LoadGrammar(new Grammar(chCommandGrammarBuilder));
      //chSRE.LoadGrammar(new DictationGrammar());
      chSRE.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(chSRESpeechDetected);
      chSRE.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(chSRESpeechRecognized);
      chSRE.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(chSRESpeechRecognitionRejected);
      chSRE.RecognizeCompleted += new EventHandler<RecognizeCompletedEventArgs>(chSRESpeechRecognizeCompleted);
      //chSRE.SetInputToDefaultAudioDevice();
      //chSRE.RecognizeAsync(RecognizeMode.Multiple);
    }

    private void enSRESpeechDetected(object sender, SpeechDetectedEventArgs e) {
      //Console.WriteLine("enSRE Begin.");
    }

    private void enSRESpeechRecognized(object sender, SpeechRecognizedEventArgs e) {
      string result = e.Result.Words[0].Text.ToLower();
      Console.WriteLine("enSRE " + e.Result.Confidence + " : " + result);
      if (e.Result.Confidence > 0.2) {
        if (isChCommand) {
          return;
        }
        if (this.textBoxAnswer.Enabled) {
          this.textBoxAnswer.Text += result;
        } else if (this.textBoxVocabulary.Enabled) {
          this.textBoxVocabulary.Focus();
          this.textBoxVocabulary.Text += result;
        }
      }
    }

    private void enSRESpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e) {
      Console.WriteLine("enSRE Fail : " + e.Result.Text + ".");
    }

    private void enSRESpeechRecognizeCompleted(object sender, RecognizeCompletedEventArgs e) {
      Console.WriteLine("enSRE Done.");
    }

    private void chSRESpeechDetected(object sender, SpeechDetectedEventArgs e) {
      //Console.WriteLine("chSRE Begin.");
    }

    private void chSRESpeechRecognized(object sender, SpeechRecognizedEventArgs e) {

      isChCommand = true;

      string result = e.Result.Words[0].Text;
      Console.WriteLine("chSRE " + e.Result.Confidence + " : " + result);
      if (result == "開始" || result == "開始測驗一") {
        this.buttonStartTest1.PerformClick();
      } else if (result == "開始測驗二") {
        this.buttonStartTest2.PerformClick();
      } else if (result == "完成" || result == "輸入" || result == "送出") {
        if (this.textBoxAnswer.Enabled) {
          this.textBoxAnswer.Focus();
          SendKeys.Send("{ENTER}");
        } else if (this.textBoxVocabulary.Enabled) {
          this.textBoxVocabulary.Focus();
          SendKeys.Send("{ENTER}");
        } else if (this.buttonFinishTest.Enabled) {
          this.buttonFinishTest.PerformClick();
        }
      } else if (result == "清除" || result == "刪除") {
        if (this.textBoxAnswer.Enabled) {
          this.textBoxAnswer.Text = "";
        } else if (this.textBoxVocabulary.Enabled) {
          this.textBoxVocabulary.Text = "";
        }
      } else if (result == "往前" || result == "往上" || result == "上一題") {
        if (this.buttonPrevQuestion.Enabled) {
          this.buttonPrevQuestion.PerformClick();
        }
      } else if (result == "往後" || result == "往下" || result == "下一題" || result == "繼續") {
        if (this.buttonNextQuestion.Enabled) {
          this.buttonNextQuestion.PerformClick();
        }
      } else if (result == "關閉" || result == "結束") {
        Application.Exit();
      } else {
        isChCommand = false;
      }
    }

    private void chSRESpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e) {
      isChCommand = false;
      Console.WriteLine("chSRE fail : " + e.Result.Text + ".");
    }

    private void chSRESpeechRecognizeCompleted(object sender, RecognizeCompletedEventArgs e) {
      Console.WriteLine("chSRE Done.");
    }

    private void speechSynthesizerInitial() {
      enSS = new SpeechSynthesizer();
      enSS.Rate = -4;
    }

    private void readByCustomFile(object sender, EventArgs e) {
      Stream inputStream;
      OpenFileDialog openFileDialog1 = new OpenFileDialog();

      openFileDialog1.InitialDirectory = @"D:\Programming\Projects\EnglishVocabularyLearner";
      openFileDialog1.Filter = "txt files (*.txt)|*.txt";
      openFileDialog1.FilterIndex = 1;
      openFileDialog1.RestoreDirectory = true;

      if (openFileDialog1.ShowDialog() == DialogResult.OK) {
        if ((inputStream = openFileDialog1.OpenFile()) != null) {
          inputStreamParser(inputStream);
          inputStream.Close();
        }
      }
    }

    private void inputStreamParser(Stream inputStream) {
      StreamReader textStream = new StreamReader(inputStream);
      int lineCount = 0, successCount = 0;
      while (textStream.Peek() >= 0) {
        lineCount++;
        String line = textStream.ReadLine();
        String[] tempStrs = Regex.Split(line, "\\ +"); // Split spaces
        List<String> strs = new List<String>();
        for (int i = 0; i < tempStrs.Length; i++) {
          if (tempStrs[i] != "" && tempStrs[i].IndexOf(" ") == -1) { // Check for exception
            strs.Add(tempStrs[i]);
          }
        }
        if (strs.Count == 2 || strs.Count == 3) { // A voc need 3 attribute, attribute 1 is score
          if ((Regex.IsMatch(strs[0], @"^[\-\+\s]?[0-9]+$") /* && Regex.IsMatch(strs[1], @"^[a-zA-Z]+$") */)) {
            vocList.Add(new Vocabulary(Int32.Parse(strs[0]), strs[1], strs.Count == 3 ? strs[2] : ""));
            successCount++;
          }
        } else if (strs.Count > 3) {
          if ((Regex.IsMatch(strs[0], @"^[\-\+\s]?[0-9]+$") /* && Regex.IsMatch(strs[1], @"^[a-zA-Z]+$") */)) {
            String tempTranslations = "";
            for (int i = 2; i < tempStrs.Length; i++) {
              tempTranslations += tempStrs[i] + " ";
            }
            vocList.Add(new Vocabulary(Int32.Parse(strs[0]), strs[1], tempTranslations));
            successCount++;
          }
        } else { // Parse fail
        }
      }
      textStream.Close();
      this.labelFileReadStatus.Text = "讀取成功數量 / 檔案行數 / 總成功數 : " + successCount + " / " + lineCount + " / " + vocList.Count;
      this.groupBoxMainOperation.Visible = true;
      this.groupBoxTestOperation.Visible = true;
      this.buttonFileRead.Text = "加入單字庫";
      vocabularyPool = new List<Vocabulary>();

      Thread thread = new Thread(vocabularyPoolHandler); // Improve the speed of getting new question
      thread.IsBackground = true; // In order to kill thread when close
      thread.Start();
    }

    private void writeNewVocabularyFile() {
      new Thread(() => {
        vocList.Sort();
        String vocabularyFileString = "";
        for (int i = 0; i < vocList.Count; i++) {
          vocabularyFileString += vocList[i].score + " " + vocList[i].text + " " + vocList[i].translation + "\n";
        }
        System.IO.File.WriteAllText("list.txt", vocabularyFileString);
        System.IO.File.WriteAllText("backup.txt", vocabularyFileString);
      }).Start();
    }

    private void vocabularyPoolHandler() {
      while (true) {
        if (checkBoxAutoClose.Checked && isTesting && isTestFinished) {
          // Do not need vocabulary anymore
          return;
        }
        if (vocList == null || vocList.Count == 0 || vocabularyPool.Count >= 10) {
          // Only need 10 in pool
          // Don't use while because need to return
          Thread.Sleep(300);
          continue;
        }
        int choosenNumber = numberChooser.getNextNumber(vocList.Count);
        vocabularyPool.Add(vocList[choosenNumber]);
        updateVocabularyInformation(vocabularyPool[vocabularyPool.Count - 1], false);
        if (isTesting) {
          UIUpdate(); // Update again
        }
        Thread.Sleep(10);
      }
    }

    private void readyToStartTest(object sender, EventArgs e) {
      testType = (sender as Button).Name[(sender as Button).Name.Length - 1] - '0'; // Get test type from button name
      TextBox senderTextBox = (testType == 1) ? this.textBoxNumberOfTest1 : this.textBoxNumberOfTest2;
      if (!Regex.IsMatch(senderTextBox.Text, @"^[0-9]+$")) { // Only accept digit
        senderTextBox.Text = "";
        return;
      }

      questionNumbers = Convert.ToInt32(senderTextBox.Text);
      if (questionNumbers == 0) {
        return;
      }

      // Hide other groups and show the test group
      this.groupBoxFileRead.Visible = false;
      this.groupBoxMainOperation.Visible = false;
      this.groupBoxTestOperation.Visible = false;
      this.groupBoxTest.Visible = true;

      // Initial
      this.textBoxAnswer.Text = "";
      this.richTextBoxQuestionDone.Text = "";
      nowQuestionNumber = lastQuestionNumber = nowWrongNumber = 0;

      questions = new List<Vocabulary>();
      answers = new List<String>();
      answerStatus = new List<bool>();
      getNextQuestion(); // First question

      this.ControlBox = false;
      isTesting = true;
      isTestFinished = false;
      UIUpdate();
    }

    private void readyToCloseTest(Object sender, EventArgs e) {
      writeNewVocabularyFile();
      if (checkBoxAutoClose.Checked) {
        this.Close();
      }
      this.groupBoxFileRead.Visible = true;
      this.groupBoxMainOperation.Visible = true;
      this.groupBoxTestOperation.Visible = true;
      this.groupBoxTest.Visible = false; // Hide other groups and show the main group

      this.ControlBox = true;
      isTesting = false;
    }

    private void UIUpdate() {
      if (isTestFinished) {
        this.buttonFinishTest.Visible = true;
      } else {
        this.buttonFinishTest.Visible = false;
      }

      String statusString, questionString;
      // Status
      if (testType == 1) {
        statusString = "進度 : " + (lastQuestionNumber + 1) + "/" + questionNumbers;
      } else {
        statusString = "答錯數量 : " + nowWrongNumber + "/" + questionNumbers;
      }
      statusString += "\n目前題目 : " + (nowQuestionNumber + 1) + "/" + (lastQuestionNumber + 1);

      // Question
      Vocabulary nowVocabulary = questions[nowQuestionNumber];
      questionString = nowVocabulary.translation + "\n";
      if (nowQuestionNumber != lastQuestionNumber || isTestFinished) { // Already done
        this.textBoxAnswer.Visible = false;

        this.richTextBoxQuestion.BackColor = answerStatus[nowQuestionNumber] ? Color.Green : Color.Red;
        questionString += nowVocabulary.text + "\n\n" + answers[nowQuestionNumber];
      } else {
        this.textBoxAnswer.Visible = true;
        questionString += nowVocabulary.text[0]; // Show the first alphabet
        for (int i = 1; i < nowVocabulary.text.Length - 1; i++) {  // Hide the others
          this.richTextBoxQuestion.BackColor = this.BackColor;
          questionString += Char.IsLetter(nowVocabulary.text[i]) ? "_" : nowVocabulary.text[i].ToString();
        }
        questionString += nowVocabulary.text[nowVocabulary.text.Length - 1];
      }

      String newTextBoxString = statusString + "\n\n" + questionString;
      if (this.richTextBoxQuestion.Text != newTextBoxString) { // Only if text is changed
        this.richTextBoxQuestion.Text = newTextBoxString;
      }
      this.vocabularyBrowser.setVocabulary(nowVocabulary);

      // Next button
      if (nowQuestionNumber != lastQuestionNumber && (testType == 2 || (nowQuestionNumber != questionNumbers - 1))) {
        this.buttonNextQuestion.Enabled = true;
      } else {
        this.buttonNextQuestion.Enabled = false;
      }

      // Prev button
      if (nowQuestionNumber != 0) {
        this.buttonPrevQuestion.Enabled = true;
      } else {
        this.buttonPrevQuestion.Enabled = false;
      }
    }

    private void nextQuestion(object sender, EventArgs e) {
      nowQuestionNumber++;
      if (nowQuestionNumber >= lastQuestionNumber) {
        nowQuestionNumber = lastQuestionNumber;
      }
      UIUpdate();
    }

    private void prevQuestion(object sender, EventArgs e) {
      nowQuestionNumber--;
      if (nowQuestionNumber < 0) {
        nowQuestionNumber = 0;
      }
      UIUpdate();
    }

    private void submitAnswer(object sender, KeyEventArgs e) {
      if (e.KeyCode != Keys.Enter || !this.textBoxAnswer.Visible) {
        return;
      }

      this.answers.Add(this.textBoxAnswer.Text);

      if (this.textBoxAnswer.Text.ToLower() == questions[nowQuestionNumber].text.ToLower()) {
        answerStatus[nowQuestionNumber] = true;
        enSS.SpeakAsync("Correct");
      } else {
        answerStatus[nowQuestionNumber] = false;
        nowWrongNumber++;
        enSS.SpeakAsync("Wrong");
      }

      enSS.SpeakAsync(questions[nowQuestionNumber].text.ToLower());

      for (int i = 0; i < vocList.Count; i++) {
        if (vocList[i].textEqualTo(questions[nowQuestionNumber])) {
          // If correct then lower the score, otherwise higher
          vocList[i].score += (answerStatus[nowQuestionNumber] ? -1 : 1) * (Math.Abs(vocList[i].score / 2) + 1);
        } else if (!answerStatus[nowQuestionNumber] && vocList[i].text.ToLower() == textBoxAnswer.Text.ToLower()) {
          // User input the other vocabulary, also higher that vocabulary
          vocList[i].score++;
        }
      }

      // Update list file
      writeNewVocabularyFile();

      this.richTextBoxQuestionDone.AppendText((answerStatus[nowQuestionNumber] ? "" : "*") + questions[nowQuestionNumber].text + "\n");

      if (testType == 1) {
        if (nowQuestionNumber == questionNumbers - 1) {
          isTestFinished = true;
        }
      } else if (testType == 2) {
        if (nowWrongNumber == questionNumbers) {
          isTestFinished = true;
        }
      }

      if (isTestFinished) {
        int correctNumbers = (lastQuestionNumber + 1) - nowWrongNumber;
        this.richTextBoxQuestionDone.AppendText("達題狀況 : " + correctNumbers + "/" + (lastQuestionNumber + 1) + "\n");
        this.richTextBoxQuestionDone.AppendText("正確率 : " + 100 * correctNumbers / (float)(lastQuestionNumber + 1) + "%");
        UIUpdate();
        return;
      }
      this.textBoxAnswer.Text = "";

      getNextQuestion();

      lastQuestionNumber++;
      /*
      if (answerStatus[nowQuestionNumber]) { // If correct than skip to the next one
        nowQuestionNumber++;
      }
       */
      UIUpdate();
    }

    private void submitVocabulary(object sender, KeyEventArgs e) {

      if (e.KeyCode != Keys.Enter || !this.textBoxVocabulary.Visible) {
        return;
      }

      enSS.SpeakAsync(this.textBoxVocabulary.Text);

      bool alreadyInList = false;
      this.buttonVocabularyOperation.Visible = false;

      Vocabulary vocabulary = new Vocabulary(this.textBoxVocabulary.Text);
      this.vocabularyBrowser.setVocabulary(vocabulary);
      updateVocabularyInformation(vocabulary, true);

      for (int i = 0; i < vocList.Count && !alreadyInList; i++)
        if (vocList[i].text.ToLower() == vocabulary.text.ToLower()) {
          alreadyInList = true;
          this.richTextBoxVocabularyInformation.Text = "此單字已在單字庫中\n\n" + vocList[i].translation;
          this.buttonVocabularyOperation.Text = "修改單字"; // Not finished yet
        }
      if (!alreadyInList) {
        this.richTextBoxVocabularyInformation.Text = "此單字未在單字庫中\n\n" + vocabulary.translation;
        this.buttonVocabularyOperation.Text = "加入單字"; // Not finished yet
      }
      this.buttonVocabularyOperation.Visible = true;
    }

    private void updateVocabularyInformation(Vocabulary vocabulary, bool updateVocabularyBrowser) {
      BackgroundWorker backgroundWorker = new BackgroundWorker();
      backgroundWorker.DoWork += (sender, args) => {
        vocabulary.setInformationFromInternet();
      };
      backgroundWorker.RunWorkerCompleted += (sender, args) => {
        if (isTesting) {
          if (vocabulary.text == questions[nowQuestionNumber].text) {
            UIUpdate();
          }
        } else if (updateVocabularyBrowser) {
          this.vocabularyBrowser.setVocabulary(vocabulary);
        }
      };
      backgroundWorker.RunWorkerAsync();
    }

    private void getNextQuestion() {
      answerStatus.Add(false);
      if (vocabularyPool.Count > 0) { // Get vocabulary from the pool
        questions.Add(vocabularyPool[0]);
        vocabularyPool.RemoveAt(0);
        return;
      }
      int choosenNumber = numberChooser.getNextNumber(vocList.Count);
      questions.Add(vocList[choosenNumber]);
      updateVocabularyInformation(questions[questions.Count - 1], false);
    }

    private void textBoxAutoFocus(object sender, EventArgs e) {
      if ((sender as TextBox).Visible) {
        (sender as TextBox).Select();
      } else {
        this.buttonNextQuestion.Select();
      }
    }

  }
}
