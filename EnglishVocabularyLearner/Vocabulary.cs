using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using HtmlAgilityPack;

namespace EnglishVocabularyLearner {
  public class Vocabulary : IComparable<Vocabulary> {
    public int score; // Using for rank
    public String text, translation; // Data from the list
    public String definition, example; // Data from Internet

    private bool alreadyGetInformationFromInternet;

    public Vocabulary(String text) {
      inital();
      this.text = text;
    }

    public Vocabulary(int score, String text) {
      inital();
      this.score = score;
      this.text = text;
    }

    public Vocabulary(int score, String text, String translation) {
      inital();
      this.score = score;
      this.text = text;
      this.translation = translation;
    }

    private void inital() {
      this.score = 0;
      this.text = "";
      this.translation = "";
      this.definition = "";
      this.example = "";
      alreadyGetInformationFromInternet = false;
    }

    public int CompareTo(Vocabulary other) {
      if (score != other.score) {
        return other.score.CompareTo(score);
      }
      int sum = 0, sumOther = 0;
      for (int i = 0; i < text.Length; i++) {
        sum += text[i];
      }
      for (int i = 0; i < other.text.Length; i++) {
        sumOther += other.text[i];
      }
      sum = sum % 31;
      sumOther = sumOther % 31;
      return sum - sumOther;
    }

    public void setInformationFromInternet() {
      if (alreadyGetInformationFromInternet) {
        return;
      }
      alreadyGetInformationFromInternet = true;
      setTranslationFromInternet();
      setDefinitionFromInternet();
      setExampleFromInternet();
    }

    private void setTranslationFromInternet() {
      if (translation != "") {
        return;
      }
      /*
      HtmlDocument doc = new HtmlWeb().Load("https://translate.google.com.tw/?hl=zh-TW&tab=wT#en/zh-TW/" + text);
      doc.OptionFixNestedTags = true;
      translation = "";
      HtmlNodeCollection root = doc.DocumentNode.SelectNodes("//span[@class='short_text']//span");
      if (root == null) {
        translation = "未找到Google翻譯";
        return;
      }
      translation = "Google翻譯 : " + root[0].InnerHtml;
      if (root[0].InnerText == null || root[0].InnerText == "" || Regex.IsMatch(root[0].InnerText, @"[a-zA-Z]+")) {
        translation = "未找到Google翻譯";
      }
      */
    }

    private void setDefinitionFromInternet() {
      HtmlDocument doc = new HtmlWeb().Load("http://wordnetweb.princeton.edu/perl/webwn?s=" + text + "&sub=Search+WordNet&o2=&o0=1&o8=1&o1=1&o7=&o5=&o9=&o6=&o3=&o4=&h=");
      doc.OptionFixNestedTags = true;
      definition = "";
      HtmlNodeCollection root = doc.DocumentNode.SelectNodes("//div[@class='form']//ul//li");
      if (root == null) {
        definition = "未找到範例";
        return;
      }
      foreach (HtmlNode rootNode in root) {
         String sentence = "";
         foreach (HtmlNode node in rootNode.ChildNodes) {
           sentence += node.InnerText;
         }
         sentence = textFilter(sentence);
         definition += sentence + "\n\n";
      }
      if (definition == "") {
        definition = "未找到範例";
      }
    }

    private void setExampleFromInternet() {
      HtmlDocument doc = new HtmlWeb().Load("http://sentence.yourdictionary.com/" + text);
      //new HtmlWeb().Load("http://www.collinsdictionary.com/dictionary/english/" + vocList[choosenNumber].text);
      //new HtmlWeb().Load("http://www.ldoceonline.com/Leisure-topic/" + vocList[choosenNumber].text);
      //new HtmlWeb().Load("http://dictionary.cambridge.org/dictionary/learner-english/" + vocList[choosenNumber].text + "?q=" + vocList[choosenNumber].text);
      doc.OptionFixNestedTags = true;
      example = "";
      HtmlNodeCollection root = doc.DocumentNode.SelectNodes("//ul[@class='example']/li");
      if (root == null) {
        example = "未找到範例";
        return;
      }
      foreach (HtmlNode rootNode in root) {
        String sentence = "";
        foreach (HtmlNode node in rootNode.ChildNodes) {
          sentence += node.InnerHtml;
        }
        if (sentence == "") {
          continue;
        }
        sentence = textFilter(sentence);
        sentence = sentence.Replace("<b>", "");
        sentence = sentence.Replace("</b>", "");
        sentence = sentence.Replace("\n", "");
        example += sentence + "\n\n";
      }
      if (example == "") {
        example = "未找到範例";
      }
    }

    private String textFilter(String sentence) { // Hide the text in a string
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
