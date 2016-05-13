using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentralForum.Client.Forum
{
    public interface IMeesageEditorParent
    {
        bool NewPostWindowVisibility { get; set; }
        void AddNewlyPostedMessage(Message message);
    }
}
