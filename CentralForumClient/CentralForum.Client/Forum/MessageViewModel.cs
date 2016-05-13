using CentralForum.Client.Messages;
using CentralForum.Client.Model;
using CentralForum.Client.Model.Entities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Models.Models;

namespace CentralForum.Client.Forum
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MessageViewModel : ViewModelBase
    {
        private Message _message;
        private ForumContext _context;
        private IDataService _service;
        public string CreationDateAsString{get; set;}

        /// <summary>
        /// Initializes a new instance of the ReplyViewModel class.
        /// </summary>
        public MessageViewModel(IDataService service, Message message, ForumContext context)
        {
            _message = message;
            _context = context;
            _service = service;

            Messenger.Default.Register<RatingUpdatedMsg>(this, msg =>
            {
                if (_message.UserGuid == msg.UserGuid)
                {
                    _message.UserRating += (int)msg.RatingValue;
                    RaisePropertyChanged(() => UserRating);
                }
            });

            CreationDateAsString = message.CreationDate.ToString("dd/MM/yyyy HH:mm");
        }

        private RelayCommand _incrementRating;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand IncrementRating
        {
            get
            {
                return _incrementRating
                    ?? (_incrementRating = new RelayCommand(
                    () =>
                    {
                        if (_service.ThumbsUp(_message.Id, _message.UserGuid, _context.UserId))
                        {
                            UpdateRating(RatingValues.Up);
                        }
                    }));
            }
        }

        private RelayCommand _decrementRating;

        public RelayCommand DecrementRating
        {
            get
            {
                return _decrementRating
                    ?? (_decrementRating = new RelayCommand(
                    () =>
                    {
                        if (_service.ThumbsDown(_message.Id, _message.UserGuid, _context.UserId))
                        {
                            UpdateRating(RatingValues.Down);
                        }
                    }));
            }
        }


        private void UpdateRating(RatingValues ratingValue)
        {
            _message.Rating += (int)ratingValue;
            RaisePropertyChanged(() => MessageRating);

            var msg = new RatingUpdatedMsg()
            {
                MessageGuid = _message.Id,
                RatingValue = ratingValue,
                UserGuid = _message.UserGuid
            };
            Messenger.Default.Send(msg);
        }

        /// <summary>
        /// Sets and gets the MessageRating property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string MessageRating
        {
            get
            {
                return _message.Rating.ToString();
            }
        }

        public string UserRating
        {
            get
            {
                return _message.UserRating.ToString();
            }
        }

        public string Title
        {
            get
            {
                return _message.Title;
            }
        }

        public string UserDisplayName
        {
            get
            {
                return _message.UserDisplayName;
            }
        }

        public string Description
        {
            get
            {
                return _message.Description;
            }
        }

    }
}