﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WTManager.Controls;

namespace WTManager.UI
{
    [System.ComponentModel.DesignerCategory("Form")]
    public partial class ConfigurationForm : WTManagerForm
    {
        public ConfigurationForm() {
            InitializeComponent();
        }

        private void ServiceConfigForm_Load(object sender, EventArgs e) {
            this.servicesListBox.Format += (s, ea) => {
                var service = (Service)ea.Value;
                ea.Value = $"{service.ServiceName} - {service.DisplayName}";
            };

            if (ConfigManager.Services != null) {
                this.servicesListBox.Items.AddRange(ConfigManager.Services.ToArray());
            }

            this.logViewerPathTb.Text = ConfigManager.Preferences.LogViewerPath;
            this.configEditorPathTb.Text = ConfigManager.Preferences.EditorPath;
        }


        private void selectConfigEditorPathBtn_Click(object sender, EventArgs e) {
            var execPath = this.RequestExecutablePath();
            if (execPath != null) {
                this.configEditorPathTb.Text = execPath;
            }
        }

        private void selectLogViewerPathBtn_Click(object sender, EventArgs e) {
            var execPath = this.RequestExecutablePath();
            if (execPath != null) {
                this.logViewerPathTb.Text = execPath;
            }
        }

        #region Services-related buttons
        private void addServiceBtn_Click(object sender, EventArgs e) {
            using (var f = new AddEditServiceForm()) {
                var result = f.ShowDialog();
                if (f.DialogResult != DialogResult.OK || f.Service == null) {
                    return;
                }
                this.servicesListBox.Items.Add(f.Service);
                SaveConfiguration();
            }
        }

        private void editServiceBtn_Click(object sender, EventArgs e) {
            var selectedService = this.servicesListBox.SelectedItem;
            if (selectedService == null) {
                return;
            }
            var index = this.servicesListBox.SelectedIndex;
            using (var f = new AddEditServiceForm((Service)selectedService)) {
                var result = f.ShowDialog();
                if (f.DialogResult != DialogResult.OK) {
                    return;
                }
                this.servicesListBox.Items[index] = f.Service;
                SaveConfiguration();
            }
        }

        private void removeServiceBtn_Click(object sender, EventArgs e) {
            var selectedService = this.servicesListBox.SelectedItem;
            if (selectedService != null) {
                var mb = MessageBox.Show("Do you really want to delete this service from list?",
                    "Removing service", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (mb == DialogResult.Yes) {
                    this.servicesListBox.Items.Remove(selectedService);
                    this.SaveConfiguration();
                }
            }
        }
        #endregion

        #region Window-related buttons
        private void OkBtn_Click(object sender, EventArgs e) {
            this.SaveConfiguration();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e) {
            this.Close();
        }
        #endregion

        #region Helper methods
        private void SaveConfiguration() {
            ConfigManager.Instance.Config.Services = this.servicesListBox.Items.OfType<Service>();
            ConfigManager.Instance.Config.Preferences.EditorPath = this.configEditorPathTb.Text;
            ConfigManager.Instance.Config.Preferences.LogViewerPath = this.logViewerPathTb.Text;
            ConfigManager.Instance.SaveConfig();
        }

        private string RequestExecutablePath() {
            var dialog = new OpenFileDialog {
                Filter = "Executable files|*.exe;*.bat;*.cmd",
                CheckFileExists = true,
                ValidateNames = true
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                var execPath = dialog.FileName;
                if (execPath != null && File.Exists(execPath)) {
                    return execPath;
                }
            }
            return null;
        }
        #endregion
    }
}
