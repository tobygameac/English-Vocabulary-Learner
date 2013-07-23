using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EnglishVocabularyLearner {
  public class Vocabulary : IComparable<Vocabulary> {
    public int score;
    public String text, translation, definition, example;

    public Vocabulary(String text) {
      this.score = 0;
      this.text = text;
      this.translation = "";
      this.definition = "";
      this.example = "";
    }

    public Vocabulary(int score, String text) {
      this.score = score;
      this.text = text;
      this.translation = "";
      this.definition = "";
      this.example = "";
    }

    public Vocabulary(int score, String text, String translation) {
      this.score = score;
      this.text = text;
      this.translation = translation;
      this.definition = "";
      this.example = "";
    }

    public bool Equals(Vocabulary other) {
      return (this.score == other.score && this.text == other.text && this.translation == other.translation);
    }

    public int CompareTo(Vocabulary other) {
      if (score != other.score) {
        return other.score.CompareTo(score);
      }
      return text.CompareTo(other.text);
    }

    public void setImformationFromInternet() {
      setTranslationString();
      setDefinitionString();
      setExampleString();
    }

    private void setTranslationString() {
      /* WebBrowser webBrowser = new WebBrowser();
      webBrowser.Navigate("https://translate.google.com.tw/?hl=zh-TW&tab=wT#en/zh-TW/" + text);
      webBrowser.ScriptErrorsSuppressed = true; // Avoid script error

      while (webBrowser.ReadyState != WebBrowserReadyState.Complete) {
        Application.DoEvents();
      }

      HtmlElement doc = webBrowser.Document.GetElementById("result_box");
      translation += doc.InnerText;
      Console.WriteLine(doc.InnerText);
      if (doc.InnerText == "") {
        translation += "未找到google翻譯";
      } */
    }

    private void setDefinitionString() {
      WebBrowser webBrowser = new WebBrowser();
      webBrowser.Navigate("http://wordnetweb.princeton.edu/perl/webwn?s=" + text + "&sub=Search+WordNet&o2=&o0=1&o8=1&o1=1&o7=&o5=&o9=&o6=&o3=&o4=&h=");
      webBrowser.ScriptErrorsSuppressed = true; // Avoid script error

      while (webBrowser.ReadyState != WebBrowserReadyState.Complete) {
        Application.DoEvents();
      }
      /* if (webBrowser.Url.AbsoluteUri != "http://dictionary.reference.com/browse" + text) {
        definition = "未找到範例";
        return;
      } */
      HtmlDocument doc = webBrowser.Document;
      definition = "";
      for (int i = 0; i < doc.All.Count; i++) {
        if (doc.All[i].TagName == "LI") {
          String sentence = doc.All[i].InnerText;
          sentence = textFilter(sentence);
          definition += sentence + "\n\n";
        }
      }
      if (definition == "") {
        definition = "未找到範例";
      }
    }

    private void setExampleString() {
      WebBrowser webBrowser = new WebBrowser();
      webBrowser.Navigate("http://sentence.yourdictionary.com/" + text);
      //webBrowser.Navigate("http://www.collinsdictionary.com/dictionary/english/" + vocList[choosenNumber].text);
      //webBrowser.Navigate("http://www.ldoceonline.com/Leisure-topic/" + vocList[choosenNumber].text);
      //webBrowser.Navigate("http://dictionary.cambridge.org/dictionary/learner-english/" + vocList[choosenNumber].text + "?q=" + vocList[choosenNumber].text);
      webBrowser.ScriptErrorsSuppressed = true; // Avoid script error

      while (webBrowser.ReadyState != WebBrowserReadyState.Complete) {
        Application.DoEvents();
      }
      HtmlDocument doc = webBrowser.Document;
      example = "";
      for (int i = 0; i < doc.All.Count; i++) {
        if (doc.All[i].GetAttribute("className") == "example") {
          //if (doc.All[i].GetAttribute("className") == "DEF") {
          //if (doc.All[i].GetAttribute("className") == "eg") {
          String sentence = doc.All[i].InnerText;
          sentence = textFilter(sentence);
          if (sentence.IndexOf("\n") != -1) {
            sentence = sentence.Replace("\n", "\n\n");
          }
          example += sentence;
        }
      }
      if (example == "") {
        example = "未找到範例";
      }
    }

    private String textFilter(String sentence) {
      String newString = text[0].ToString();
      for (int i = 1; i < text.Length - 1; i++)
        newString += "_";
      newString += text[text.Length - 1].ToString();
      if (sentence != null && sentence != "" && sentence.IndexOf(text) != -1) {
        sentence = sentence.Replace(text, newString);
      }
      return sentence;
    }
  }
}
