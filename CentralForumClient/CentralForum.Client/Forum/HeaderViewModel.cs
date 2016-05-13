using CentralForum.Client.Model;
using CentralForum.Client.Model.Entities;
using GalaSoft.MvvmLight;
using Models.Models;

namespace CentralForum.Client.Forum
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class HeaderViewModel : ViewModelBase
    {
        private ForumViewModel _parent;

        /// <summary>
        /// Initializes a new instance of the HeaderViewModel class.
        /// </summary>
        public HeaderViewModel(ForumViewModel parent)
        {
            _parent = parent;
            _isPublicChecked = true;
        }

        /// <summary>
        /// The <see cref="IsPublicMessagesChecked" /> property's name.
        /// </summary>
        public const string IsPublicMessagesCheckedPropertyName = "IsPublicMessagesChecked";

        private bool _isPublicChecked = false;

        /// <summary>
        /// Sets and gets the IsPublicMessagesChecked property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsPublicMessagesChecked
        {
            get
            {
                return _isPublicChecked;
            }

            set
            {
                if (_isPublicChecked == value)
                {
                    return;
                }

                _isPublicChecked = value;

                if(_isPublicChecked)
                {
                    MessageType = MessageType | MessageType.Public;
                }
                else
                {
                    MessageType = MessageType & (~MessageType.Public);
                }

                RaisePropertyChanged(IsPublicMessagesCheckedPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsPrivateMessagesChecked" /> property's name.
        /// </summary>
        public const string IsPrivateMessagesCheckedPropertyName = "IsPrivateMessagesChecked";

        private bool _isPrivateChecked = false;

        /// <summary>
        /// Sets and gets the IsPrivateMessagesChecked property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsPrivateMessagesChecked
        {
            get
            {
                return _isPrivateChecked;
            }

            set
            {
                if (_isPrivateChecked == value)
                {
                    return;
                }

                _isPrivateChecked = value;

                if (_isPrivateChecked)
                {
                    MessageType = MessageType | MessageType.Private;
                }
                else
                {
                    MessageType = MessageType & (~MessageType.Private);
                }

                RaisePropertyChanged(IsPrivateMessagesCheckedPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="IsHowToMessagesChecked" /> property's name.
        /// </summary>
        public const string IsHowToMessagesCheckedPropertyName = "IsHowToMessagesChecked";

        private bool _isHowToChecked = false;

        /// <summary>
        /// Sets and gets the IsHowToMessagesChecked property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsHowToMessagesChecked
        {
            get
            {
                return _isHowToChecked;
            }

            set
            {
                if (_isHowToChecked == value)
                {
                    return;
                }

                _isHowToChecked = value;

                if (_isHowToChecked)
                {
                    MessageType = MessageType | MessageType.HowTo;
                }
                else
                {
                    MessageType = MessageType & (~MessageType.HowTo);
                }

                RaisePropertyChanged(IsHowToMessagesCheckedPropertyName);
            }
        }

        private  MessageType _messageType = MessageType.Public;

        public MessageType MessageType
        {
            get 
            { 
                return _messageType; 
            }
            set
            {
                _messageType = value;
                _parent.LoadPosts();
            }
        }

        /// <summary>
        /// The <see cref="SearchText" /> property's name.
        /// </summary>      
        public const string SearchTextPropertyName = "SearchText";

        private string _searchText = "";

        /// <summary>
        /// Sets and gets the SearchText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SearchText
        {
            get
            {
                return _searchText;
            }

            set
            {
                if (_searchText == value)
                {
                    return;
                }

                _searchText = value;
                _parent.LoadPosts();
                RaisePropertyChanged(SearchTextPropertyName);
            }
        }
    }
}