using System.Diagnostics;
using System.Security.AccessControl;

namespace PDEVerifyGUI {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
            // 加载上次保存的目录
            LoadLastStr();
        }

        /// <summary>
        /// 根目录路径
        /// </summary>
        string RootDirPath = "";
        /// <summary>
        /// 单文件路径
        /// </summary>
        string FilePath = "";

        /// <summary>
        /// 选择文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelectDirectory_Click(object sender, EventArgs e) {
            // 显示文件夹选择对话框
            if (FolderBrowserDialog.ShowDialog() == DialogResult.OK) {
                DirTextBox.Text = FolderBrowserDialog.SelectedPath;
                RootDirPath = FolderBrowserDialog.SelectedPath;
                SaveLastStr(); // 保存选择的目录
            }
        }

        /// <summary>
        /// 处理根目录下所有文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DirFixButton_Click(object sender, EventArgs e) {
            if (RootDirPath != "") {
                // 调用 CalHash 类的 CalDir 方法
                await CalHash.CalDirAsync(RootDirPath);

                // 显示完成消息框
                MessageBox.Show(this, "所有都修复完成啦！", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
            } else {
                //显示 错误警告框
                MessageBox.Show(this, "选择资源根目录！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileSelectButton_Click(object sender, EventArgs e) {
            // 创建并配置 OpenFileDialog 对话框
            OpenFileDialog openFileDialog = new() {
                Title = "选择 .cache 文件",
                Filter = "Cache files (*.cache)|*.cache",
                CheckFileExists = true, // 确保文件存在
                ValidateNames = true // 确保文件名有效
            };

            // 显示文件选择对话框
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                FileTextBox.Text = openFileDialog.FileName;
                FilePath = openFileDialog.FileName;
                SaveLastStr(); // 保存选择的目录
            }
        }

        /// <summary>
        /// 处理单个文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FileFixButton_Click(object sender, EventArgs e) {
            // (╯°□°）╯︵ ┻━┻)
            if (RootDirPath == "" | FilePath == "") {
                MessageBox.Show(this, "请先择必要文件或文件夹！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                // 调用 CalHash 类的 CalFile 方法
                await CalHash.CalFileAsync(RootDirPath, FilePath);

                // 显示完成消息框
                MessageBox.Show(this, "单文件修复完成", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        /// <summary>
        /// 进入推拽文件
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
        /// 推拽文件
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
                        MessageBox.Show("请拖放一个有效的 .cache 文件。", "无效文件", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                } else {
                    MessageBox.Show("无效的文件路径。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 加载上次打开的文件
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
        /// 保存上次使用的目录和文件
        /// </summary>
        private void SaveLastStr() {
            Properties.Settings.Default.LastSelectedDirectory = DirTextBox.Text;
            Properties.Settings.Default.LastSelectedFile = FileTextBox.Text;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// 关闭程序是保存
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e) {
            base.OnFormClosing(e);
            SaveLastStr();
        }
    }
}
