using System.Diagnostics;
using System.Security.AccessControl;

namespace PDEVerifyGUI {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
            // �����ϴα����Ŀ¼
            LoadLastStr();
        }

        /// <summary>
        /// ��Ŀ¼·��
        /// </summary>
        string RootDirPath = "";
        /// <summary>
        /// ���ļ�·��
        /// </summary>
        string FilePath = "";

        /// <summary>
        /// ѡ���ļ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelectDirectory_Click(object sender, EventArgs e) {
            // ��ʾ�ļ���ѡ��Ի���
            if (FolderBrowserDialog.ShowDialog() == DialogResult.OK) {
                DirTextBox.Text = FolderBrowserDialog.SelectedPath;
                RootDirPath = FolderBrowserDialog.SelectedPath;
                SaveLastStr(); // ����ѡ���Ŀ¼
            }
        }

        /// <summary>
        /// �����Ŀ¼�������ļ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DirFixButton_Click(object sender, EventArgs e) {
            if (RootDirPath != "") {
                // ���� CalHash ��� CalDir ����
                await CalHash.CalDirAsync(RootDirPath);

                // ��ʾ�����Ϣ��
                MessageBox.Show(this, "���ж��޸��������", "���", MessageBoxButtons.OK, MessageBoxIcon.None);
            } else {
                //��ʾ ���󾯸��
                MessageBox.Show(this, "ѡ����Դ��Ŀ¼��", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ѡ���ļ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileSelectButton_Click(object sender, EventArgs e) {
            // ���������� OpenFileDialog �Ի���
            OpenFileDialog openFileDialog = new() {
                Title = "ѡ�� .cache �ļ�",
                Filter = "Cache files (*.cache)|*.cache",
                CheckFileExists = true, // ȷ���ļ�����
                ValidateNames = true // ȷ���ļ�����Ч
            };

            // ��ʾ�ļ�ѡ��Ի���
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                FileTextBox.Text = openFileDialog.FileName;
                FilePath = openFileDialog.FileName;
                SaveLastStr(); // ����ѡ���Ŀ¼
            }
        }

        /// <summary>
        /// �������ļ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FileFixButton_Click(object sender, EventArgs e) {
            // (�s����㣩�s�� �ߩ���)
            if (RootDirPath == "" | FilePath == "") {
                MessageBox.Show(this, "�������Ҫ�ļ����ļ��У�", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                // ���� CalHash ��� CalFile ����
                await CalHash.CalFileAsync(RootDirPath, FilePath);

                // ��ʾ�����Ϣ��
                MessageBox.Show(this, "���ļ��޸����", "���", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        /// <summary>
        /// ������ק�ļ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileTextBox_DragEnter(object sender, DragEventArgs e) {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true) {
                e.Effect = DragDropEffects.Copy;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// ��ק�ļ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileTextBox_DragDrop(object sender, DragEventArgs e) {
            if (e.Data?.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0) {
                string filePath = files[0];
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)) {
                    string? extension = Path.GetExtension(filePath);
                    if (extension != null && string.Equals(extension, ".cache", StringComparison.OrdinalIgnoreCase)) {
                        FileTextBox.Text = filePath;
                        FilePath = filePath;
                    } else {
                        MessageBox.Show("���Ϸ�һ����Ч�� .cache �ļ���", "��Ч�ļ�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                } else {
                    MessageBox.Show("��Ч���ļ�·����", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// �����ϴδ򿪵��ļ�
        /// </summary>
        private void LoadLastStr() {
            string lastDir = Properties.Settings.Default.LastSelectedDirectory;
            if (!string.IsNullOrEmpty(lastDir) && Directory.Exists(lastDir)) {
                DirTextBox.Text = lastDir;
                RootDirPath = lastDir;
            }

            string lastFile = Properties.Settings.Default.LastSelectedFile;
            if (!string.IsNullOrEmpty(lastFile) && File.Exists(lastFile)) {
                FileTextBox.Text = lastFile;
                FilePath = lastFile;
            }
        }

        /// <summary>
        /// �����ϴ�ʹ�õ�Ŀ¼���ļ�
        /// </summary>
        private void SaveLastStr() {
            Properties.Settings.Default.LastSelectedDirectory = DirTextBox.Text;
            Properties.Settings.Default.LastSelectedFile = FileTextBox.Text;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// �رճ����Ǳ���
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e) {
            base.OnFormClosing(e);
            SaveLastStr();
        }
    }
}
