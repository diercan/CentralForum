using GalaSoft.MvvmLight;

namespace CentralForum.Client.Forum
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class PostBarViewModel : ViewModelBase
    {
        private PostViewModel _parent;

        /// <summary>
        /// Initializes a new instance of the PostBarViewModel class.
        /// </summary>
        public PostBarViewModel(PostViewModel parent)
        {
            _parent = parent;
            _parent.PropertyChanged += _parent_PropertyChanged;
        }

        private void _parent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "NewPostWindowVisibility")
            {
                RaisePropertyChanged(() => ReplayEditorVisibility);
            }
        }

        /// <summary>
        /// Sets and gets the ReplayEditorVisibility property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool ReplayEditorVisibility
        {
            get
            {
                return _parent.NewPostWindowVisibility;
            }
            set
            {
                if (this._parent.NewPostWindowVisibility == value) return;

                this._parent.NewPostWindowVisibility = value;
                RaisePropertyChanged(()=>ReplayEditorVisibility);
            }
        }
    }
}