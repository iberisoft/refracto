using System;
using System.Windows;
using Ookii.Dialogs.Wpf;
using Refracto.Data;

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

        public bool ConfirmDelete(Timeline timeline)
        {
            return MessageBox.Show(string.Format("Delete '{0}'?", timeline.Id), MessageBoxTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public void WarnExist(Timeline timeline)
        {
            MessageBox.Show(string.Format("'{0}' already exists.", timeline.Id), MessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void Error(Exception ex)
        {
            MessageBox.Show(ex.Message, MessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public string BrowseFolder()
        {
            var dialog = new VistaFolderBrowserDialog();
            return dialog.ShowDialog() == true ? dialog.SelectedPath : null;
        }
    }
}
