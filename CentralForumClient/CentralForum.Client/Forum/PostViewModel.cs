using CentralForum.Client.Model;
using CentralForum.Client.Model.Entities;
using GalaSoft.MvvmLight;
using Models.Models;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

namespace CentralForum.Client.Forum
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class PostViewModel : ViewModelBase, IMeesageEditorParent
    {
        private bool _isVisible;
        private Message _post;
        private PostBarViewModel _postBar;
        private ObservableCollection<MessageViewModel> _replyMessages;
        private bool _repliesViewVisibility;
        private IDataService _service;
        private ForumContext _context;

        public MessageViewModel MainPostVM { get; private set; }

        /// <summary>
        /// Initializes a new instance of the PostViewModel class.
        /// </summary>
        public PostViewModel(IDataService service, ForumContext context, Message post, List<Message> replyMessages)
        {
            _service = service;
            _context = context;
            _post = post;
            _replyMessages = new ObservableCollection<MessageViewModel>();
            replyMessages.ForEach(m=>_replyMessages.Add(new MessageViewModel(service, m, context)));
            MainPostVM = new MessageViewModel(service, post, context);
            _postBar = new PostBarViewModel(this);
            MessageEditorVM = new MessageEditorViewModel(this, service, context, _post);
        }

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                return _post.Title;
            }
        }


        /// <summary>
        /// Sets and gets the Description property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Description
        {
            get
            {
                return _post.Description;
            }
        }

        /// <summary>
        /// Sets and gets the Description property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public PostBarViewModel PostBarVM
        {
            get
            {
                return _postBar;
            }
        }

        /// <summary>
        /// Sets and gets the Description property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<MessageViewModel> PostReplies
        {
            get
            {
                return _replyMessages;
            }
        }

        /// <summary>
        /// Sets and gets the ReplayEditorVisibility property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool NewPostWindowVisibility
        {
            get
            {
                return this._isVisible;
            }
            set
            {
                if (this._isVisible == value) return;

                this._isVisible = value;
                RaisePropertyChanged(() => NewPostWindowVisibility);
            }
        }

        public MessageEditorViewModel MessageEditorVM { get; set; }
        

        /// <summary>
        /// Sets and gets the ReplayEditorVisibility property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool RepliesViewVisibility
        {
            get
            {
                return this._repliesViewVisibility;
            }
            set
            {
                if (this._repliesViewVisibility == value) return;

                this._repliesViewVisibility = value;
                RaisePropertyChanged(() => RepliesViewVisibility);
            }
        }

        public void AddNewlyPostedMessage(Message message)
        {
            PostReplies.Add(new MessageViewModel(_service, message, _context));
        }
    }
}