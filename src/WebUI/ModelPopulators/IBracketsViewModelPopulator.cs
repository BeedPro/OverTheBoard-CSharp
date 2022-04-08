using System.Collections.Generic;
using OverTheBoard.Data.Entities;
using OverTheBoard.ObjectModel;
using OverTheBoard.WebUI.Models;

namespace OverTheBoard.WebUI.ModelPopulators
{
    public interface IBracketsViewModelPopulator
    {
        BracketsViewModel Populate(string currentUserId, Tournament tournament, List<ChessGame> games, Dictionary<string, OverTheBoardUser> users);
    }
}