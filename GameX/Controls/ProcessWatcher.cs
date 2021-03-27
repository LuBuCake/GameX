using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameX.Controls
{
    public partial class ProcessWatcher : DevExpress.XtraEditors.XtraUserControl
    {
        public App RuntimeApp { get; set; }

        public ProcessWatcher(App Runtime)
        {
            InitializeComponent();
            RuntimeApp = Runtime;
        }
    }
}
