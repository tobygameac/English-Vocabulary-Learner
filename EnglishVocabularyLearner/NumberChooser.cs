using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVocabularyLearner {
  class NumberChooser {
    private Random random;

    public NumberChooser() {
      random = new Random();
    }

    public int getNextNumber(int max) {
      int dice = random.Next(10);
      int choosenNumber = 0;
      switch (dice) {
        case 0: // 10% chance from vocabulary with top 0.5% score
          choosenNumber = random.Next((int)(max * 0.005));
          break;
        case 1:
        case 2:
        case 3:
        case 4:
        case 5: // 50% chance from vocabulary with top 5% score
          choosenNumber = random.Next((int)(max * 0.05));
          break;
        case 6:
        case 7:
        case 8: // 30% chance from vocabulary with top 50% score
          choosenNumber = random.Next((int)(max * 0.5));
          break;
        case 9: // 10% chance from vocabulary with all score
          choosenNumber = random.Next((int)(max * 1.0));
          break;
      }
      return choosenNumber;
    }
  }
}
