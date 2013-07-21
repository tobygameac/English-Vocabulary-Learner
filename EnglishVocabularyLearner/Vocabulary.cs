using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnglishVocabularyLearner {
  public class Vocabulary : IComparable<Vocabulary> {
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

    public void setDefinitionsString() {
      WebBrowser webBrowser = new WebBrowser();
      webBrowser.Navigate("http://www.collinsdictionary.com/dictionary/english/" + text + "?showCookiePolicy=true");
      webBrowser.ScriptErrorsSuppressed = true; // Avoid script error

      while (webBrowser.ReadyState != WebBrowserReadyState.Interactive && webBrowser.ReadyState != WebBrowserReadyState.Complete) {
        Application.DoEvents();
      }
      HtmlDocument doc = webBrowser.Document;
      definition = "";
      for (int i = 0; i < doc.All.Count; i++) {
        if (doc.All[i].GetAttribute("className") == "definitions hom-subsec") {
          String sentence = doc.All[i].InnerText;
          if (sentence.IndexOf(text) != -1) {
            sentence = sentence.Replace(text, "__________");
          }
          definition += sentence;
        }
      }
      if (definition == "") {
        definition = "未找到範例";
      }
    }

    public void setExampleString() {
      WebBrowser webBrowser = new WebBrowser();
      webBrowser.Navigate("http://sentence.yourdictionary.com/" + text);
      //webBrowser.Navigate("http://www.collinsdictionary.com/dictionary/english/" + vocList[choosenNumber].text);
      //webBrowser.Navigate("http://www.ldoceonline.com/Leisure-topic/" + vocList[choosenNumber].text);
      //webBrowser.Navigate("http://dictionary.cambridge.org/dictionary/learner-english/" + vocList[choosenNumber].text + "?q=" + vocList[choosenNumber].text);
      webBrowser.ScriptErrorsSuppressed = true; // Avoid script error

      while (webBrowser.ReadyState != WebBrowserReadyState.Interactive && webBrowser.ReadyState != WebBrowserReadyState.Complete) {
        Application.DoEvents();
      }
      HtmlDocument doc = webBrowser.Document;
      example = "";
      for (int i = 0; i < doc.All.Count; i++) {
        if (doc.All[i].GetAttribute("className") == "example") {
          //if (doc.All[i].GetAttribute("className") == "DEF") {
          //if (doc.All[i].GetAttribute("className") == "eg") {
          String sentence = doc.All[i].InnerText;
          if (sentence.IndexOf(text) != -1) {
            sentence = sentence.Replace(text, "__________");
          }
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

    public int score;
    public String text, translation, definition, example;
  }
}
