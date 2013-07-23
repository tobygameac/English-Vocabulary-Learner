﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnglishVocabularyLearner {
  public partial class VocabularyViewer : GroupBox {
    private Vocabulary vocabulary;

    public VocabularyViewer() {
      InitializeComponent();
    }

    public void setVocabulary(Vocabulary vocabulary) {
      this.vocabulary = vocabulary;
      this.richTextBoxDefinition.Text = vocabulary.definition;
      this.richTextBoxExample.Text = vocabulary.example;
    }
  }
}
