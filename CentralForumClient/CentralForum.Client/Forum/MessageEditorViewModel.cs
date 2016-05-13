using CentralForum.Client.Model;
using CentralForum.Client.Model.Entities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Models.Models;
using System;

namespace CentralForum.Client.Forum
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MessageEditorViewModel : ViewModelBase
    {
        private IMeesageEditorParent _parent;
        private IDataService _service;
        private ForumContext _context;
        private Message _repliedMessage;

        /// <summary>
        /// Initializes a new instance of the ReplyEditorViewModel class.
        /// </summary>
        public MessageEditorViewModel(IMeesageEditorParent parent, IDataService service, ForumContext context, Message repliedMessage)
            : this(parent, service, context)
        {
            _repliedMessage = repliedMessage;
            MessageType = repliedMessage.MessageType;
        }

        /// <summary>
        /// Initializes a new instance of the ReplyEditorViewModel class.
        /// </summary>
        public MessageEditorViewModel(IMeesageEditorParent parent, IDataService service, ForumContext context)
        {
            _service = service;
            _parent = parent;
            _context = context;
            MessageType = MessageType.Public;
        }

        private RelayCommand _cancelPostCommand;

        /// <summary>
        /// The <see cref="TextMessage" /> property's name.
        /// </summary>
        public const string TextMessagePropertyName = "TextMessage";

        private string _textMesssage = "";

        public MessageType MessageType { get; set; }

        /// <summary>
        /// Sets and gets the TextMessage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TextMessage
        {
            get
            {
                return _textMesssage;
            }

            set
            {
                if (_textMesssage == value)
                {
                    return;
                }

                _textMesssage = value;
                RaisePropertyChanged(TextMessagePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private string _title = "";

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                if (_title == value)
                {
                    return;
                }

                _title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        /// <summary>
        /// Gets the CancelPost.
        /// </summary>
        public RelayCommand CancelPost
        {
            get
            {
                return _cancelPostCommand
                    ?? (_cancelPostCommand = new RelayCommand(
                    () =>
                    {
                        ClearEditor();
                    }));
            }
        }

        private void ClearEditor()
        {
            TextMessage = "";
            Title = "";
            _parent.NewPostWindowVisibility = false;
        }

        private RelayCommand _postMessage;

        /// <summary>
        /// Gets the PostMessage.
        /// </summary>
        public RelayCommand PostMessage
        {
            get
            {
                return _postMessage
                    ?? (_postMessage = new RelayCommand(
                    () =>
                    {
                        var newMsg = new Message()
                        {
                            Description = TextMessage,
                            Title = Title,
                            Id = Guid.NewGuid(),
                            MessageType = MessageType,
                            PracticeGuid = _context.PracticeId,
                            PracticeName = _context.PracticeDisplayName,
                            UserDisplayName = _context.UserDisplayName,
                            UserGuid = _context.UserId,
                            TopicName = _context.TopicName,
                            CreationDate = DateTime.Now
                        };

                        if (_repliedMessage != null)
                        {
                            newMsg.ParentId = _repliedMessage.Id;
                        }
                        _service.PostMessage(newMsg);
                        ClearEditor();
                        _parent.AddNewlyPostedMessage(newMsg);
                    },
                    () => !string.IsNullOrWhiteSpace(TextMessage)));
            }
        }
    }
}