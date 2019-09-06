using Caliburn.Micro;
using System.IO;
using System.Linq;

namespace Refracto.ViewModels
{
    class CreateTimelineViewModel : Screen
    {
        string m_TimelineName = "";

        public string TimelineName
        {
            get => m_TimelineName;
            set
            {
                if (Set(ref m_TimelineName, value))
                {
                    NotifyOfPropertyChange(() => CanAccept);
                }
            }
        }

        public bool CanAccept => TimelineName != "" && Path.GetInvalidFileNameChars().All(ch => !TimelineName.Contains(ch));

        public void Accept()
        {
            TryClose(true);
        }
    }
}
