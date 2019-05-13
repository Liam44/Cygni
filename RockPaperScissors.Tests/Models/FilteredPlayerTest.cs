using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockPaperScissors.Models;

namespace RockPaperScissors.Tests.Models
{
    [TestClass]
    public class FilteredPlayerTest
    {
        [TestMethod]
        public void NewFilteredPlayer_NameNull_MoveNull()
        {
            // Arrange
            string name = null;
            string move= null;

            // Act
            FilteredPlayer filteredPlayer = new FilteredPlayer(name, move);

            // Assert
            Assert.AreEqual(string.Empty, filteredPlayer.Name);
            Assert.AreEqual(string.Empty, filteredPlayer.Move);
        }

        [TestMethod]
        public void NewFilteredPlayer_NameEmpty_MoveEmpty()
        {
            // Arrange
            string name= string.Empty;
            string move = string.Empty;

            // Act
            FilteredPlayer filteredPlayer = new FilteredPlayer(name, move);

            // Assert
            Assert.AreEqual(string.Empty, filteredPlayer.Name);
            Assert.AreEqual(string.Empty, filteredPlayer.Move);
        }

        [TestMethod]
        public void NewFilteredPlayer_NameValid_MoveValid()
        {
            // Arrange
            string name = "John";
            string move = "SomeMove";

            // Act
            FilteredPlayer filteredPlayer = new FilteredPlayer(name, move);

            // Assert
            Assert.AreEqual(name, filteredPlayer.Name);
            Assert.AreEqual(move, filteredPlayer.Move);
        }
    }
}
