using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace EnglishVocabularyLearner {

  public partial class Form1 : Form {
    private List<Vocabulary> vocList = new List<Vocabulary>();

    public Form1() {
      InitializeComponent();

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

      this.textBoxAnswer.KeyDown += new KeyEventHandler(this.submitAnswer);
      this.textBoxAnswer.VisibleChanged += new EventHandler(this.textBoxAutoFocus);
    }

    private void Form1_Load(object sender, EventArgs e) {
      this.DoubleBuffered = true;
      Form.CheckForIllegalCrossThreadCalls = false;
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
      int lineCount = 0;
      while (textStream.Peek() >= 0) {
        lineCount++;
        String line = textStream.ReadLine();
        String[] tempStrs = Regex.Split(line, "\\ +"); // Split spaces
        List<String> strs = new List<String>();
        for (int i = 0; i < tempStrs.Length; i++) {
          if (tempStrs[i] != "" && tempStrs[i].IndexOf(" ") == -1) { // Check for exception status
            strs.Add(tempStrs[i]);
          }
        }
        if (strs.Count == 3 && // A voc need 3 attribute, attribute 1 is score
          (Regex.IsMatch(strs[0], @"^[\-\+\s]*[0-9]+$") /* && Regex.IsMatch(strs[1], @"^[a-zA-Z]+$") */)) {
          vocList.Add(new Vocabulary(Int32.Parse(strs[0]), strs[1], strs[2]));
        } else {
        }
      }
      textStream.Close();
      this.labelFileReadStatus.Text = "讀取成功數量 / 檔案行數 : " + vocList.Count + " / " + lineCount;
      this.groupBoxMainOperation.Visible = true;
      this.buttonFileRead.Text = "更換單字庫";
    }

    private void writeNewVocabularyFile() {
      String vocabularyFileString = "";
      for (int i = 0; i < vocList.Count; i++) {
        vocabularyFileString += vocList[i].score + "         " + vocList[i].text + "         " + vocList[i].translation + "\n";
      }
      System.IO.File.WriteAllText("list.txt", vocabularyFileString);
    }

    // Variables for test
    private int testType;
    private int questionNumbers;
    private int nowQuestionNumber;
    private int lastQuestionNumber;
    private int nowWrongNumber; // Test type 2
    private bool isTestFinished;

    // Record status of every question
    private List<Vocabulary> questions;
    private List<String> answers;
    private List<bool> answerStatus;

    private void readyToStartTest(object sender, EventArgs e) {
      testType = (sender as Button).Name[(sender as Button).Name.Length - 1] - '0';
      TextBox senderTextBox = (testType == 1) ? this.textBoxNumberOfTest1 : this.textBoxNumberOfTest2;
      if (!Regex.IsMatch(senderTextBox.Text, @"^[0-9]+$")) { // Only accept digit
        senderTextBox.Text = "";
        return;
      } else {
        questionNumbers = Convert.ToInt32(senderTextBox.Text);
        if (questionNumbers == 0) {
          return;
        }
      }

      // Hide other groups and show the test group
      this.groupBoxFileRead.Visible = false;
      this.groupBoxMainOperation.Visible = false;
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
      isTestFinished = false;
      testHandler();
    }

    private void readyToCloseTest(Object sender, EventArgs e) {
      writeNewVocabularyFile();
      if (checkBoxAutoClose.Checked) {
        this.Close();
      }
      this.groupBoxFileRead.Visible = true;
      this.groupBoxMainOperation.Visible = true;
      this.groupBoxTest.Visible = false; // Hide other groups and show the main group

      this.ControlBox = true;
      isTestFinished = true;
    }

    private void testHandler() {
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
          questionString += nowVocabulary.text + "\n" + answers[nowQuestionNumber];
        } else {
          this.textBoxAnswer.Visible = true;
          questionString += nowVocabulary.text[0]; // Show the first alphabet
          for (int i = 1; i < nowVocabulary.text.Length; i++) {  // Hide the others
            this.richTextBoxQuestion.BackColor = this.BackColor;
            questionString += Char.IsLetter(nowVocabulary.text[i]) ? "_" : nowVocabulary.text[i].ToString();
          }
        }
        String newTextBoxString = statusString + "\n\n" + questionString;
        if (this.richTextBoxQuestion.Text != newTextBoxString) { // Only Change if text is changed
          this.richTextBoxQuestion.Text = newTextBoxString;
        }
        if (this.richTextBoxExapmle.Text != questions[nowQuestionNumber].example) {
          this.richTextBoxExapmle.Text = questions[nowQuestionNumber].example;
        }

        // Next button
        if (nowQuestionNumber != lastQuestionNumber && (testType == 2 || (nowQuestionNumber != questionNumbers - 1))) {
          this.buttonNextQuestion.Visible = true;
        } else {
          this.buttonNextQuestion.Visible = false;
        }

        // Prev button
        if (nowQuestionNumber != 0) {
          this.buttonPrevQuestion.Visible = true;
        } else {
          this.buttonPrevQuestion.Visible = false;
        }
    }

    private void nextQuestion(object sender, EventArgs e) {
      nowQuestionNumber++;
      if (nowQuestionNumber >= lastQuestionNumber) {
        nowQuestionNumber = lastQuestionNumber;
      }
      testHandler();
    }

    private void prevQuestion(object sender, EventArgs e) {
      nowQuestionNumber--;
      if (nowQuestionNumber < 0) {
        nowQuestionNumber = 0;
      }
      testHandler();
    }

    private void submitAnswer(object sender, KeyEventArgs e) {
      if (e.KeyCode != Keys.Enter || !this.textBoxAnswer.Visible) {
        return;
      }

      this.answers.Add(this.textBoxAnswer.Text);
      if (this.textBoxAnswer.Text == questions[nowQuestionNumber].text) {
        answerStatus[nowQuestionNumber] = true;
      } else {
        answerStatus[nowQuestionNumber] = false;
        nowWrongNumber++;
      }

      for (int i = 0; i < vocList.Count; i++)
        if (vocList[i].text == questions[nowQuestionNumber].text) {
          // If correct then lower the score, otherwise higher
          vocList[i].score += (answerStatus[nowQuestionNumber] ? -1 : 1) * (Math.Abs(vocList[i].score / 2) + 1);
        } else if (!answerStatus[nowQuestionNumber] && vocList[i].text == textBoxAnswer.Text) {
          // Answer the other vocabulary, also higher
          vocList[i].score++;
        }

      // Update
      vocList.Sort();
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
        testHandler();
        return;
      }
      this.textBoxAnswer.Text = "";
      getNextQuestion();
      lastQuestionNumber++;
      if (answerStatus[nowQuestionNumber]) { // If correct than skip to the next one
        nowQuestionNumber++;
      }
      testHandler();
    }

    private void textBoxAutoFocus(object sender, EventArgs e) {
      if ((sender as TextBox).Visible) {
        (sender as TextBox).Select();
      } else {
        this.buttonNextQuestion.Select();
      }
    }

    private Random random = new Random(); // Prevent from mutil declare

    private void getNextQuestion() {
      int dice = random.Next(10);
      int choosenNumber = 0;
      switch (dice) {
        case 0: // 10% chance from vocabulary with top 1% score
          choosenNumber = random.Next((int)(vocList.Count * 0.01));
          break;
        case 1:
        case 2:
        case 3:
        case 4: // 40% chance from vocabulary with top 15% score
          choosenNumber = random.Next((int)(vocList.Count * 0.15));
          break;
        case 5:
        case 6:
        case 7:
        case 8: // 40% chance from vocabulary with top 50% score
          choosenNumber = random.Next((int)(vocList.Count * 0.5));
          break;
        case 9: // 10% chance from vocabulary with all score
          choosenNumber = random.Next((int)(vocList.Count * 1.0));
          break;
      }
      answerStatus.Add(false);
      vocList.Sort();
      vocList[choosenNumber].getExampleString();
      questions.Add(vocList[choosenNumber]);
    }
  }
}
