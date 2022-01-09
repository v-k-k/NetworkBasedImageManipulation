using System.Collections.Generic;
using System.Linq;
using System;

namespace NetworkBasedImageManipulation.Workers
{
    public enum ProcessState
    {
        Inactive,
        Active,
        ActiveHost,
        ActivePort,
        Paused,
        SavedHost,
        SavedPort,
        ToComplete,
        Completed,
        Terminated
    }

    public enum Command
    {
        Begin,
        Back,
        Pause,
        Port,
        Host,
        SaveHost,
        SavePort,
        Complete,
        Exit
    }
    public class CommandWorker
    {
        public struct Data
        {
            public bool Updated;
            public string State;

            private string __value;

            public string Value
            {
                get { return __value; }
                set
                {
                    __value = value;
                    Updated = Value.Length > 0;
                    State = Updated ? "(saved)" : "";
                }
            }

            public Data(string value = "")
            {
                __value = value;
                Updated = false;
                State = "";
            }
        }

        internal class StateTransition
        {
            readonly ProcessState CurrentState;
            readonly Command Command;

            public StateTransition(ProcessState currentState, Command command)
            {
                CurrentState = currentState;
                Command = command;
            }

            public override int GetHashCode()
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
            }
        }

        Dictionary<StateTransition, ProcessState> transitions;
        Dictionary<string, Command[]> availableCommands;
        Dictionary<string, string> commandsAutocompletion;

        public Dictionary<ProcessState, string> StateMenu;
        public Data IP = new Data();
        public Data PORT = new Data();

        public ProcessState CurrentState { get; private set; }
        public string CurrentMenu { get; private set; }

        public CommandWorker()
        {
            CurrentState = ProcessState.Inactive;
            transitions = new Dictionary<StateTransition, ProcessState>
            {
                { new StateTransition(ProcessState.Inactive, Command.Exit), ProcessState.Terminated },
                { new StateTransition(ProcessState.Inactive, Command.Pause), ProcessState.Paused },
                { new StateTransition(ProcessState.Inactive, Command.Begin), ProcessState.Active },

                { new StateTransition(ProcessState.Paused, Command.Exit), ProcessState.Terminated },
                { new StateTransition(ProcessState.Paused, Command.Back), ProcessState.Inactive },

                { new StateTransition(ProcessState.Active, Command.Exit), ProcessState.Terminated },
                { new StateTransition(ProcessState.Active, Command.Back), ProcessState.Inactive },
                { new StateTransition(ProcessState.Active, Command.Host), ProcessState.ActiveHost },
                { new StateTransition(ProcessState.Active, Command.Port), ProcessState.ActivePort },

                { new StateTransition(ProcessState.ActiveHost, Command.Exit), ProcessState.Terminated },
                { new StateTransition(ProcessState.ActiveHost, Command.Back), ProcessState.Active },
                { new StateTransition(ProcessState.ActiveHost, Command.SaveHost), ProcessState.SavedHost },

                { new StateTransition(ProcessState.SavedHost, Command.Exit), ProcessState.Terminated },
                { new StateTransition(ProcessState.SavedHost, Command.Back), ProcessState.ActiveHost },
                { new StateTransition(ProcessState.SavedHost, Command.Port), ProcessState.ActivePort },
                { new StateTransition(ProcessState.SavedHost, Command.Complete), ProcessState.ToComplete },

                { new StateTransition(ProcessState.ActivePort, Command.Exit), ProcessState.Terminated },
                { new StateTransition(ProcessState.ActivePort, Command.Back), ProcessState.Active },
                { new StateTransition(ProcessState.ActivePort, Command.SavePort), ProcessState.SavedPort },

                { new StateTransition(ProcessState.SavedPort, Command.Exit), ProcessState.Terminated },
                { new StateTransition(ProcessState.SavedPort, Command.Back), ProcessState.ActivePort },
                { new StateTransition(ProcessState.SavedPort, Command.Host), ProcessState.ActiveHost },
                { new StateTransition(ProcessState.SavedPort, Command.Complete), ProcessState.ToComplete },

                { new StateTransition(ProcessState.ToComplete, Command.Exit), ProcessState.Terminated },
                { new StateTransition(ProcessState.ToComplete, Command.Host), ProcessState.SavedHost },
                { new StateTransition(ProcessState.ToComplete, Command.Port), ProcessState.SavedPort },
                { new StateTransition(ProcessState.ToComplete, Command.Back), ProcessState.Active },
                { new StateTransition(ProcessState.ToComplete, Command.Complete), ProcessState.Completed },

                { new StateTransition(ProcessState.Completed, Command.Exit), ProcessState.Terminated },
                { new StateTransition(ProcessState.Completed, Command.Back), ProcessState.ToComplete }
            };
            StateMenu = new Dictionary<ProcessState, string>
            {
                { ProcessState.Inactive, "You're in the main menu:\n\r\t- set\n\r\t- get\n\r\t- quit\n\r> " },
                { ProcessState.Paused, "Host: {0}\n\rPort: {1}\n\r\t- back\n\r\t- quit\n\r> " },

                { ProcessState.Active, "Set the endpoint params:\n\r\t- host {0}\n\r\t- port {1}\n\r\t- back\n\r\t- quit\n\r> " },
                { ProcessState.ActiveHost, "Please enter the host IP:\n\r\t- back\n\r\t- quit\n\r> " },
                { ProcessState.ActivePort, "Please enter the port number:\n\r\t- back\n\r\t- quit\n\r> " },

                { ProcessState.SavedHost, "Saved host {0}:\n\r\t- port {1}{2}\n\r\t- back\n\r\t- quit\n\r> " },
                { ProcessState.SavedPort, "Saved port {0}:\n\r\t- host {1}{2}\n\r\t- back\n\r\t- quit\n\r> " },
                { ProcessState.ToComplete, "The {0}:{1} endpoint was saved:{2}\n\r\t- back\n\r\t- quit\n\r> " },

                { ProcessState.Completed, "Setup completed:\n\r\t- back\n\r\t- quit\n\r> " },
                { ProcessState.Terminated, "Good bye !!!" },
            };
            availableCommands = new Dictionary<string, Command[]>
            {
                { "set", new Command[] { Command.Begin } },
                { "get", new Command[] { Command.Pause } },
                { "quit", new Command[] { Command.Exit } },
                { "host", new Command[] { Command.Host } },
                { "port", new Command[] { Command.Port } },
                { "", new Command[] { Command.SaveHost, Command.SavePort } },
                { "apply", new Command[] { Command.Complete } },
                { "back", new Command[] { Command.Back } }               
            };
            commandsAutocompletion = new Dictionary<string, string>
            {
                { "s", "set" }, { "se", "set" },
                { "g", "get" }, { "ge", "get" },
                { "h", "host"}, {"ho", "host"}, {"hos", "host"},
                { "p", "port"}, {"po", "port"}, {"pot", "port"},
                { "a", "apply"}, {"ap", "apply"}, {"app", "apply"}, {"appl", "apply"},
                { "q", "quit"}, {"qu", "quit"}, {"qui", "quit"},
                { "b", "back"}, {"ba", "back"}, {"bac", "back"}
            };
        }

        private ProcessState GetNext(Command command)
        {
            StateTransition transition = new StateTransition(CurrentState, command);
            ProcessState nextState;
            if (!transitions.TryGetValue(transition, out nextState))
                throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
            return nextState;
        }

        public string MoveNext(Command command)
        {
            CurrentState = GetNext(command);
            string apply = IP.Updated && PORT.Updated ? "\n\r\t- apply" : "";
            switch (CurrentState)
            {
                case ProcessState.Paused:
                    return string.Format(StateMenu[CurrentState], IP.Value, PORT.Value);
                case ProcessState.Active:
                    return string.Format(StateMenu[CurrentState], IP.State, PORT.State);
                case ProcessState.SavedHost:
                    return string.Format(StateMenu[CurrentState], IP.Value, PORT.State, apply);
                case ProcessState.SavedPort:
                    return string.Format(StateMenu[CurrentState], PORT.Value, IP.State, apply);
                case ProcessState.ToComplete:
                    return string.Format(StateMenu[CurrentState], IP.Value, PORT.Value, apply);
                default:
                    return StateMenu[CurrentState];
            }
        }

        public string Autocomplete(string input)
        {
            if (commandsAutocompletion.ContainsKey(input))
                return commandsAutocompletion[input];
            return "";
        }

        public string ProcessInput(string input)
        {
            Command command;
            if (input.Any(c => char.IsDigit(c)))
            {
                switch (CurrentState)
                {
                    case ProcessState.ActivePort:
                        PORT.Value = input;
                        command = availableCommands[""][1];
                        break;
                    default:
                        IP.Value = input;
                        command = availableCommands[""][0];
                        break;
                }
                CurrentMenu = MoveNext(command);
            }
            else if (availableCommands.ContainsKey(input))
            {
                command = availableCommands[input][0];
                CurrentMenu = MoveNext(command);
            }
            return CurrentMenu;
        }
    }
}
