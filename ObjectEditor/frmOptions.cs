using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ObjectEditor
{
    public partial class frmOptions : Form
    {
        private Config Cfg;
        public frmOptions(Config Cfg)
        {
            this.Cfg = Cfg;
            InitializeComponent();
        }

        private void SetControls(bool ToUI)
        {
            FOCommon.Utils.SetControl(txtServerFolder, ref Cfg.PathServer, ToUI);
            FOCommon.Utils.SetControl(chkLockTabs, ref Cfg.LockTabs, ToUI);
            FOCommon.Utils.SetControl(chkSwitchTab, ref Cfg.SwitchTab, ToUI);
            FOCommon.Utils.SetControl(chkTranslateInterface, ref Cfg.TranslateLanguage, ToUI);
            FOCommon.Utils.SetControl(chkStripPrefix, ref Cfg.StripPrefix, ToUI);
            FOCommon.Utils.SetControl(chkResizeOnResize, ref Cfg.ResizeOnResize, ToUI);
            FOCommon.Utils.SetControl(chkShowItemPidDefine, ref Cfg.ShowItemPidDefine, ToUI);
            FOCommon.Utils.SetControl(chkDefineBeforeId, ref Cfg.ItemPidDefineBeforeId, ToUI);
            FOCommon.Utils.SetControl(chkFormatWithSpace, ref Cfg.FormatWithSpace, ToUI);
            FOCommon.Utils.SetControl(chkShowWholeFilePath, ref Cfg.ShowWholeFilePath, ToUI);
            FOCommon.Utils.SetControl(chkLoadGraphics, ref Cfg.LoadGraphics, ToUI);

            if (!String.IsNullOrEmpty(Cfg.PathGraphics))
            {
                if (ToUI)
                {
                    lstGraphicFiles.Items.Clear();
                    string[] paths = Cfg.PathGraphics.Split(";".ToCharArray());
                    foreach (string path in paths)
                        if (!String.IsNullOrEmpty(path))
                            lstGraphicFiles.Items.Add(path);
                }
                else
                {
                    List<String> paths = new List<string>();
                    foreach (string path in lstGraphicFiles.Items)
                        paths.Add(path);
                    Cfg.PathGraphics = String.Join(";", paths.ToArray());
                }
            }

            if (ToUI) cmbLanguage.Text = Cfg.PathLanguage; else Cfg.PathLanguage = cmbLanguage.Text;
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            if(cmbLanguage.Items.Count==0)
                cmbLanguage.Items.AddRange(Directory.GetFiles("." + Path.DirectorySeparatorChar, "*.lang"));
            SetControls(true);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SetControls(false);
            Cfg.Save();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool OpenDialogPath(string Filter, ref string Text)
        {
            OpenFileDialog OpenDefines = new OpenFileDialog();
            OpenDefines.Filter = Filter;
            OpenDefines.RestoreDirectory = true;
            if (OpenDefines.ShowDialog() == DialogResult.OK)
            {
                Text = (OpenDefines.FileName);
                return true;
            }
            return false;
        }

        private void btnServerRoot_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog OpenDataFolder = new FolderBrowserDialog();
            if (OpenDataFolder.ShowDialog() == DialogResult.OK)
            {
                txtServerFolder.Text = OpenDataFolder.SelectedPath;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenGraphicsFiles = new OpenFileDialog();
            OpenGraphicsFiles.Multiselect = true;
            OpenGraphicsFiles.RestoreDirectory = true;
            OpenGraphicsFiles.Filter = "Graphics Archives (*.dat;*.zip)|*.dat;*.zip|All files (*.*)|*.*";
            if (OpenGraphicsFiles.ShowDialog() == DialogResult.OK)
                foreach (String file in OpenGraphicsFiles.FileNames)
                {
                    if (lstGraphicFiles.Items.Contains(file))
                        continue;
                    lstGraphicFiles.Items.Add(file);
                }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int j = lstGraphicFiles.SelectedItems.Count;
            for(int i = 0; i < j;i++)
            {
                lstGraphicFiles.Items.Remove(lstGraphicFiles.SelectedItems[0]);
            }
        }
    }
}
