﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace EnglishVocabularyLearner {
  public class Vocabulary : IComparable<Vocabulary> {
    public int score; // Using for rank
    public String text, translation; // Data from the list
    public String definition, example; // Data from Internet

    private WebBrowser webBrowser;
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
      webBrowser = new WebBrowser();
      alreadyGetInformationFromInternet = true;
      setTranslationFromInternet();
      setDefinitionFromInternet();
      setExampleFromInternet();
    }

    private void setTranslationFromInternet() {
      if (translation != "") {
        return;
      }
      
      webBrowser.Navigate("https://translate.google.com.tw/?hl=zh-TW&tab=wT#en/zh-TW/" + text);
      webBrowser.ScriptErrorsSuppressed = true; // Avoid script error

      while (webBrowser.ReadyState != WebBrowserReadyState.Complete) {
        Application.DoEvents();
      }

      HtmlElement doc = webBrowser.Document.GetElementById("result_box");
      
      translation = "Google翻譯 : " + doc.InnerText;
      Console.WriteLine(translation);
      if (doc.InnerText == null || doc.InnerText == "" || Regex.IsMatch(doc.InnerText, @"[a-zA-Z]+")) {
        translation = "未找到Google翻譯";
      }
    }

    private void setDefinitionFromInternet() {
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

    private void setExampleFromInternet() {
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
