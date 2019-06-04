using System;
using Refracto.Data;

namespace Refracto
{
    public interface IDialogManager
    {
        bool? ConfirmSave();

        bool ConfirmDelete(Timeline timeline);

        void WarnExist(Timeline timeline);

        void Error(Exception ex);

        string BrowseFolder();
    }
}
