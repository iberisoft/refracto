using System;

namespace Refracto.Services
{
    public interface IDialogManager
    {
        bool? ConfirmSave();

        bool ConfirmDelete(object obj);

        void WarnExist(object obj);

        void Error(Exception ex);

        string BrowseFolder(string path);
    }
}
