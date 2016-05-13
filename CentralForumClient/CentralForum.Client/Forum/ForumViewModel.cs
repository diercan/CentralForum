using System.Linq;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using CentralForum.Client.Model.Entities;
using CentralForum.Client.Model;
using Models.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;

namespace CentralForum.Client.Forum
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ForumViewModel : ViewModelBase, IMeesageEditorParent
    {
        private IDataService _service;
        private ForumContext _context;
        public HeaderViewModel HeaderVM{get; private set;}

        /// <summary>
        /// Initializes a new instance of the ForumViewModel class.
        /// </summary>
        public ForumViewModel(IDataService service, ForumContext context)
        {
            _service = service;
            _context = context;
            MessageEditorVM = new MessageEditorViewModel(this, service, context);
            HeaderVM = new HeaderViewModel(this);

            LoadPosts();
        }

        internal void LoadPosts()
        {
            var posts = _service.GetPosts(_context.TopicName, HeaderVM.MessageType, _context.PracticeId);
            _posts.Clear();
            foreach (var postGroup in posts.Where(p => 
                    string.IsNullOrWhiteSpace(HeaderVM.SearchText) || 
                    p.Description.ToLower().Contains(HeaderVM.SearchText.ToLower()) ||
                    p.Title.ToLower().Contains(HeaderVM.SearchText.ToLower()) 
                 )
                .GroupBy(m => m.ParentId ?? m.Id))
            {
                var mainPost = postGroup.SingleOrDefault(p => p.ParentId == null);
                if (mainPost == null)
                {
                    mainPost = posts.FirstOrDefault(p => p.Id == postGroup.Key);
                }
                _posts.Add(new PostViewModel(_service, _context,
                            mainPost,
                            postGroup.Where(p => p.ParentId != null).ToList())
                );
            }

            //set meessage type for new post editor
            MessageEditorVM.MessageType = GetMessagetType(HeaderVM.MessageType);
        }

        private MessageType GetMessagetType(MessageType messageType)
        {
            if((messageType & MessageType.Private) == MessageType.Private)
            {
                return MessageType.Private;
            }
            else if ((messageType & MessageType.HowTo) == MessageType.HowTo)
            {
                return MessageType.HowTo;
            }
            else if ((messageType & MessageType.Public) == MessageType.Public)
            {
                return MessageType.Public;
            }

            return MessageType.Public;
        }

        public void AddNewlyPostedMessage(Message message)
        {
            _posts.Add(new PostViewModel(_service, _context, message, new List<Message>()));
        }

        public MessageEditorViewModel MessageEditorVM { get; set; }

        private ObservableCollection<PostViewModel> _posts = new ObservableCollection<PostViewModel>();

        /// <summary>
        /// Sets and gets the Posts property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<PostViewModel> Posts
        {
            get
            {
                return _posts;
            }

            set
            {
                if (_posts == value)
                {
                    return;
                }

                _posts = value;
                RaisePropertyChanged(() => Posts);
            }
        }


        /// <summary>
        /// The <see cref="NewPostWindowVisibility" /> property's name.
        /// </summary>
        public const string NewPostWindowVisibilityPropertyName = "NewPostWindowVisibility";

        private bool _newPostWindowVisibility = false;

        /// <summary>
        /// Sets and gets the NewPostWindowVisibility property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool NewPostWindowVisibility
        {
            get
            {
                return _newPostWindowVisibility;
            }

            set
            {
                if (_newPostWindowVisibility == value)
                {
                    return;
                }

                _newPostWindowVisibility = value;
                RaisePropertyChanged(() => NewPostWindowVisibility);
            }
        }

        private RelayCommand _addNewPostCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand AddNewPost
        {
            get
            {
                return _addNewPostCommand
                    ?? (_addNewPostCommand = new RelayCommand(
                    () =>
                    {
                        NewPostWindowVisibility = true;
                    }));
            }
        }

        public bool NewPostWindowVisibility1
        {
            get
            {
                return _newPostWindowVisibility;
            }

            set
            {
                _newPostWindowVisibility = value;
            }
        }

        private RelayCommand _refresh;

        /// <summary>
        /// Gets the Refresh.
        /// </summary>
        public RelayCommand Refresh
        {
            get
            {
                return _refresh
                    ?? (_refresh = new RelayCommand(
                    () =>
                    {
                        LoadPosts();
                    }));
            }
        }
    }
}