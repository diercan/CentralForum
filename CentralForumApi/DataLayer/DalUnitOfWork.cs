using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Context;
using Models.Models;

namespace DataLayer
{
    public class DalUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private Repository<Rating> _ratingRepository;
        private Repository<Message> _messageRepository;

        public DalUnitOfWork(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public Repository<Rating> RatingRepository
        {
            get
            {
                return _ratingRepository ?? (_ratingRepository = new Repository<Rating>(_dbContext));
            }
        }

        public Repository<Message> MessageRepository
        {
            get
            {
                return _messageRepository ?? (_messageRepository = new Repository<Message>(_dbContext));
            }
        }

    }
}
