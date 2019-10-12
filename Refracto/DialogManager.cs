using Ookii.Dialogs.Wpf;
using Refracto.Services;
using System;
using System.Windows;

namespace Refracto
{
    class DialogManager : IDialogManager
    {
        private static string MessageBoxTitle => Application.Current.MainWindow.Title;

        private static bool? ToBoolean(MessageBoxResult result)
        {
            switch (result)
            {
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
                default:
                    return null;
            }
        }

        public bool? ConfirmSave()
        {
            return ToBoolean(MessageBox.Show("Save new data?", MessageBoxTitle, MessageBoxButton.YesNoCancel, MessageBoxImage.Question));
        }

        public bool ConfirmDelete(object obj)
        {
            return MessageBox.Show(string.Format("Delete '{0}'?", obj), MessageBoxTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public void WarnExist(object obj)
        {
            MessageBox.Show(string.Format("'{0}' already exists.", obj), MessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void Error(Exception ex)
        {
            MessageBox.Show(ex.Message, MessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public string BrowseFolder(string path)
        {
            var dialog = new VistaFolderBrowserDialog();
            dialog.SelectedPath = path;
            return dialog.ShowDialog() == true ? dialog.SelectedPath : null;
        }
    }
}
