using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace kaisen {
  class res {
    string filename;
    string time_start;
    string time_end;
    string winner;

    public res() {

    }
    // create history into file
    public void create_history_game(string filename, string time_start, string time_end, string min, string sec, string msec, string winner) {
      this.filename = filename;
      this.time_start = time_start;
      this.time_end = time_end;
      this.winner = winner;
      try {
        //using (FileStream fs = File.Create(filename))
        using (FileStream fs = new FileStream(filename, FileMode.Append, FileAccess.Write)) {
          byte[] info = new UTF8Encoding(true).GetBytes(time_start + " - " + time_end + " - " + min + ":" + sec + ":" + msec + " winner: " + winner + "\n");
          fs.Write(info, 0, info.Length);

        }
      } catch (Exception ex) {
        Console.WriteLine(ex.ToString());
      }

    }
  }
}