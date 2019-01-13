using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;

namespace SLMM.Api.Test
{
    [TestClass]
    public class SmartLawnMowingMachineTest
    {
        struct Result
        {
            public string Orientation;
            public uint X;
            public uint Y;
        }

        private Mock<Garden> FakeGardenSetUp(int width, int length)
        {
            var garden = new Mock<Garden>((uint)width, (uint)length);
            garden.Setup(g => g.IsValidPosition(It.IsAny<uint>(), It.IsAny<uint>())).Returns(true);
            garden.Setup(g => g.IsValidPosition(It.Is<uint>(x => x < 1 || x > width), It.IsAny<uint>())).Returns(false);
            garden.Setup(g => g.IsValidPosition(It.IsAny<uint>(), It.Is<uint>(y => y < 1 || y > length))).Returns(false);
            garden.Setup(g => g.IsValidPosition(It.Is<uint>(x => x < 1 || x > width), It.Is<uint>(y => y < 1 || y > length))).Returns(false);
            return garden;
        }

        #region TEST BORDERS
        [TestMethod]
        public void MoveWestShouldNotWorkWhenOntheWestBorder()
        {
            // Arrange
            Mock<Garden> garden = FakeGardenSetUp(10, 10);

            // Act
            var slmm = new SmartLawnMowingMachine(garden.Object, 2, 1, "east");
            slmm.Move(Command.MoveAnticlockwise);
            slmm.Move(Command.MoveAnticlockwise);
            slmm.Move(Command.MoveOneStepForward);

            Result expected = new Result
            {
                Orientation = "west",
                X = 1,
                Y = 1
            };

            (uint X, uint Y, string Orientation) location = slmm.GetPosition();

            Result actual = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MoveNordShouldNotWorkWhenOntheNordBorder()
        {
            // Arrange
            Mock<Garden> garden = FakeGardenSetUp(100, 10);

            // Act
            var slmm = new SmartLawnMowingMachine(garden.Object, 7, 2, "east");

            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveAnticlockwise);
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveOneStepForward);

            Result expected = new Result
            {
                Orientation = "nord",
                X = 8,
                Y = 1
            };

            (uint X, uint Y, string Orientation) location = slmm.GetPosition();

            Result actual = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MoveEastShouldNotWorkWhenOntheEastBorder()
        {
            // Arrange
            Mock<Garden> garden = FakeGardenSetUp(3, 10);

            // Act
            var slmm = new SmartLawnMowingMachine(garden.Object, 2, 5, "east");
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveOneStepForward);

            Result expected = new Result
            {
                Orientation = "east",
                X = 3,
                Y = 5
            };

            (uint X, uint Y, string Orientation) location = slmm.GetPosition();

            Result actual = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MoveSouthShouldNotWorkWhenOntheSouthBorder()
        {
            // Arrange
            Mock<Garden> garden = FakeGardenSetUp(10, 3);

            // Act
            var slmm = new SmartLawnMowingMachine(garden.Object, 5, 1, "east");
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveClockwise);
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveOneStepForward);

            Result expected = new Result
            {
                Orientation = "south",
                X = 6,
                Y = 3
            };

            (uint X, uint Y, string Orientation) location = slmm.GetPosition();

            Result actual = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region TEST TURNS
        [TestMethod]
        public void SUTTurnClockwiseOk()
        {
            // Arrange
            Mock<Garden> garden = FakeGardenSetUp(10, 10);

            // Act
            var slmm = new SmartLawnMowingMachine(garden.Object, 2, 1, "east");
            slmm.Move(Command.MoveClockwise);
            slmm.Move(Command.MoveClockwise);
            slmm.Move(Command.MoveClockwise);

            Result expectedPartial = new Result
            {
                Orientation = "nord",
                X = 2,
                Y = 1
            };

            (uint X, uint Y, string Orientation) location = slmm.GetPosition();

            Result actualPartial = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };

            slmm.Move(Command.MoveClockwise);

            Result expected = new Result
            {
                Orientation = "east",
                X = 2,
                Y = 1
            };

            location = slmm.GetPosition();

            Result actual = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };

            // Assert
            Assert.AreEqual(expectedPartial, actualPartial);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SUTTurnAnticlockwiseOk()
        {
            // Arrange
            Mock<Garden> garden = FakeGardenSetUp(10, 10);

            // Act
            var slmm = new SmartLawnMowingMachine(garden.Object, 2, 1, "east");
            slmm.Move(Command.MoveAnticlockwise);
            slmm.Move(Command.MoveAnticlockwise);
            slmm.Move(Command.MoveAnticlockwise);

            Result expectedPartial = new Result
            {
                Orientation = "south",
                X = 2,
                Y = 1
            };

            (uint X, uint Y, string Orientation) location = slmm.GetPosition();

            Result actualPartial = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };

            slmm.Move(Command.MoveAnticlockwise);

            Result expected = new Result
            {
                Orientation = "east",
                X = 2,
                Y = 1
            };

            location = slmm.GetPosition();

            Result actual = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };

            // Assert
            Assert.AreEqual(expectedPartial, actualPartial);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SUTTurnClockwiseAndAnticlockwiseOk()
        {
            // Arrange
            Mock<Garden> garden = FakeGardenSetUp(10, 10);

            // Act
            var slmm = new SmartLawnMowingMachine(garden.Object, 2, 1, "east");
            slmm.Move(Command.MoveClockwise);

            Result expectedPartial = new Result
            {
                Orientation = "south",
                X = 2,
                Y = 1
            };

            (uint X, uint Y, string Orientation) location = slmm.GetPosition();

            Result actualPartial = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };

            slmm.Move(Command.MoveAnticlockwise);
            slmm.Move(Command.MoveAnticlockwise);

            Result expected = new Result
            {
                Orientation = "nord",
                X = 2,
                Y = 1
            };

            location = slmm.GetPosition();

            Result actual = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };

            // Assert
            Assert.AreEqual(expectedPartial, actualPartial);
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region TEST MOVES
        [TestMethod]
        public void SUTMoveXAxis()
        {
            // Arrange
            Mock<Garden> garden = FakeGardenSetUp(10, 10);

            // Act
            var slmm = new SmartLawnMowingMachine(garden.Object, 3, 1, "east");
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveAnticlockwise);
            slmm.Move(Command.MoveAnticlockwise);
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveOneStepForward);

            Result expected = new Result
            {
                Orientation = "west",
                X = 2,
                Y = 1
            };

            (uint X, uint Y, string Orientation) location = slmm.GetPosition();

            Result actual = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SUTMoveYAxis()
        {
            // Arrange
            Mock<Garden> garden = FakeGardenSetUp(10, 10);

            // Act
            var slmm = new SmartLawnMowingMachine(garden.Object, 3, 3, "south");
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveAnticlockwise);
            slmm.Move(Command.MoveAnticlockwise);
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveOneStepForward);

            Result expected = new Result
            {
                Orientation = "nord",
                X = 3,
                Y = 2
            };

            (uint X, uint Y, string Orientation) location = slmm.GetPosition();

            Result actual = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SUTMoveXYAxes()
        {
            // Arrange
            Mock<Garden> garden = FakeGardenSetUp(10, 10);

            // Act
            var slmm = new SmartLawnMowingMachine(garden.Object, 3, 3, "south");
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveAnticlockwise);
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveClockwise);
            slmm.Move(Command.MoveOneStepForward);
            slmm.Move(Command.MoveClockwise);
            slmm.Move(Command.MoveOneStepForward);

            Result expected = new Result
            {
                Orientation = "west",
                X = 4,
                Y = 6
            };

            (uint X, uint Y, string Orientation) location = slmm.GetPosition();

            Result actual = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region TEST REACTIVITY - I WOULD NOT TEST THIS IN A REAL IMPLEMENTATION - ONLY FOR EXERCISE
        [TestMethod]
        public void SUTIsReactiveDuringMove()
        {
            // Arrange
            Mock<Garden> garden = FakeGardenSetUp(100, 100);

            // Act
            var slmm = new SmartLawnMowingMachine(garden.Object, 50, 50, "east");

            Result actual;
            Result expectedBeforeAndDuringMoving = new Result { X = 50, Y = 50, Orientation = "east" };
            Result expectedEndMoving = new Result { X = 51, Y = 50, Orientation = "east" };

            Thread t = new Thread(LocMove);
            t.Start(new Args
            {
                Slmm = slmm,
                Command = Command.MoveOneStepForward
            });

            (uint X, uint Y, string Orientation) location = slmm.GetPosition();
            actual = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };
            if (t.IsAlive)
            {
                // Assert
                Assert.AreEqual(expectedBeforeAndDuringMoving, actual);
            }

            location = slmm.GetPosition();
            actual = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };
            if (t.IsAlive)
            {
                // Assert
                Assert.AreEqual(expectedBeforeAndDuringMoving, actual);
            }

            while (t.IsAlive) { }

            location = slmm.GetPosition();
            actual = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };

            // Assert
            Assert.AreEqual(expectedEndMoving, actual);
        }

        [TestMethod]
        public void SUTIsReactiveDuringTurn()
        {
            // Arrange
            Mock<Garden> garden = FakeGardenSetUp(100, 100);

            // Act
            var slmm = new SmartLawnMowingMachine(garden.Object, 50, 50, "east");

            Result actual;
            Result expectedBeforeAndDuringMoving = new Result { X = 50, Y = 50, Orientation = "east" };
            Result expectedEndMoving = new Result { X = 50, Y = 50, Orientation = "south" };

            Thread t = new Thread(LocMove);
            t.Start(new Args
            {
                Slmm = slmm,
                Command = Command.MoveClockwise
            });

            (uint X, uint Y, string Orientation) location = slmm.GetPosition();
            actual = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };
            if (t.IsAlive)
            {
                // Assert
                Assert.AreEqual(expectedBeforeAndDuringMoving, actual);
            }

            location = slmm.GetPosition();
            actual = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };
            if (t.IsAlive)
            {
                // Assert
                Assert.AreEqual(expectedBeforeAndDuringMoving, actual);
            }

            while (t.IsAlive) { }

            location = slmm.GetPosition();
            actual = new Result
            {
                Orientation = location.Orientation.ToLower(),
                X = location.X,
                Y = location.Y
            };

            // Assert
            Assert.AreEqual(expectedEndMoving, actual);
        }

        #region PRIVATE METHODS FOR SUPPORTING REACTIVITY TESTS

        private struct Args
        {
            public SmartLawnMowingMachine Slmm { get; set; }
            public Command Command { get; set; }
        }

        private void LocMove(object args)
        {
            Args? arguments = args as Args?;
            if (arguments == null)
            {
                return;
            }

            arguments.Value.Slmm.Move(arguments.Value.Command);
        }
        #endregion
        #endregion
    }
}
