using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;


namespace EnglishVocabularyLearner {

  public partial class EnglishVocabularyLeaner {

    private List<Vocabulary> list = new List<Vocabulary>();
    private String[] urls = {"http://www.taiwantestcentral.com/WordList/WordListByName.aspx?MainCategoryID=17&Letter=", // 學測
                             "http://www.taiwantestcentral.com/WordList/WordListByName.aspx?MainCategoryID=14&Letter=", // 指考
                             "http://www.taiwantestcentral.com/WordList/WordListByName.aspx?MainCategoryID=4"}; // GEPT
    public void GetVocabularyList() {
      WebBrowser webBrowser = new WebBrowser();
      for (int type = 0; type < urls.Length; type++) {
        for (char c = 'A'; c <= 'Z'; c++) {
          Console.WriteLine(c);
          Console.WriteLine(list.Count);
          webBrowser.Navigate(urls[type] + c);
          webBrowser.ScriptErrorsSuppressed = true; // Avoid script error

          while (webBrowser.ReadyState != WebBrowserReadyState.Complete) {
            Application.DoEvents();
          }

          HtmlDocument doc = webBrowser.Document;

          String text = "", translation = "";
          int level = 1;
          for (int i = 0; i < doc.All.Count; i++) {
            if (doc.All[i].GetAttribute("className").Length > 8 && doc.All[i].GetAttribute("className").Substring(0, 8) == "nowrap w") {
              level = doc.All[i].GetAttribute("className")[8] - '0';
              text = doc.All[i].InnerText;
            }
            if (doc.All[i].GetAttribute("className") == "Chinese") {
              translation = doc.All[i].InnerText;
            }
            if (text != "" && translation != "") {
              list.Add(new Vocabulary((level - 1) * 3, text, translation));
              text = "";
              translation = "";
            }
          }
        }
      }
      list.Sort();
      String vocabularyFileString = "";
      for (int i = 0; i < list.Count; i++) {
        if (i > 0 && list[i].text == list[i - 1].text) { // Repeat
          continue;
        }
        vocabularyFileString += list[i].score + "         " + list[i].text + "         " + list[i].translation + "\n";
      }
      System.IO.File.WriteAllText("listFromInternet.txt", vocabularyFileString);
    }
  }

}