namespace EventHorizon.Blazor.Mascot.Actions;

using EventHorizon.Blazor.Mascot.Behaviors;
using EventHorizon.Blazor.Mascot.Character;

public class StandardAction
    : MascotAction
{
    public StandardAction(
        string trigger,
        MascotAvatar avatar
    ) : base(
        trigger,
        avatar
    )
    {
    }

    public override Task Run(
        InteractionKey interactionKey,
        EnvironmentItem environment
    )
    {
        var interaction = interactionKey.Value;
        if (interaction == "exit_left")
        {
            _avatar.Place(
                environment.Left + 1,
                _avatar.Position.Y
            );
            _avatar.CancelAction();

            RandomSelectorBehavior.Run(
                _avatar,
                new CharacterBehavior
                {
                    States = new List<string>
                    {
                        "climb_up",
                        "climb_down",
                    },
                    Data = new Dictionary<string, CharacterBehaviorState>
                    {
                        ["climb_up"] = new CharacterBehaviorState
                        {
                            Name = "climb",
                            Repeat = 1,
                            Prob = 0.8d,
                        },
                        ["climb_down"] = new CharacterBehaviorState
                        {
                            Name = "climb",
                            Repeat = 1,
                            RevM = 1,
                            Prob = 0.2d,
                        },
                    }
                }
            );
        }
        else if (interaction == "exit_right")
        {
            _avatar.Place(
                environment.Right - 1,
                _avatar.Position.Y
            );
            _avatar.CancelAction();

            RandomSelectorBehavior.Run(
                _avatar,
                new CharacterBehavior
                {
                    States = new List<string>
                    {
                        "climb_up",
                        "climb_down",
                    },
                    Data = new Dictionary<string, CharacterBehaviorState>
                    {
                        ["climb_up"] = new CharacterBehaviorState
                        {
                            Name = "climb",
                            Repeat = 1,
                            RevH = 1,
                            Prob = 0.8d,
                        },
                        ["climb_down"] = new CharacterBehaviorState
                        {
                            Name = "climb",
                            Repeat = 1,
                            RevH = 1,
                            RevM = 1,
                            Prob = 0.2d,
                        },
                    }
                }
            );

        }
        else if (interaction == "exit_top")
        {
            // exit_top_left
            if (Math.Abs(_avatar.Position.X - environment.Left) <
                Math.Abs(_avatar.Position.X - environment.Right)
            )
            {
                _avatar.Place(
                    _avatar.Position.X,
                    environment.Top + 1
                );
                _avatar.CancelAction();

                _avatar.Action(
                    "walk",
                    2,
                    1,
                    0,
                    0,
                    () =>
                    {
                        _avatar.Place(
                            _avatar.Position.X,
                            _avatar.Position.Y
                        );
                        if (_avatar.Config.Behavior.TryGetValue(
                            "exit_top_left",
                            out var behavior
                        ))
                        {
                            RandomSelectorBehavior.Run(
                                _avatar,
                                behavior
                            );
                        }
                        else
                        {
                            _avatar.Action(
                                "action1",
                                1,
                                1
                            );
                            _avatar.Action(
                                "fall",
                                40
                            );
                        }
                    });
            }
            // exit_top_right
            else
            {
                _avatar.Place(
                    _avatar.Position.X,
                    environment.Top + 1
                );
                _avatar.CancelAction();

                _avatar.Action(
                    "walk",
                    2,
                    0,
                    0,
                    0,
                    () =>
                    {
                        _avatar.Place(
                            _avatar.Position.X,
                            _avatar.Position.Y
                        );
                        if (_avatar.Config.Behavior.TryGetValue(
                            "exit_top_right",
                            out var behavior
                        ))
                        {
                            RandomSelectorBehavior.Run(
                                _avatar,
                                behavior
                            );
                        }
                        else
                        {
                            _avatar.Action(
                                "action1",
                                1,
                                1
                            );
                            _avatar.Action(
                                "fall",
                                40
                            );
                        }
                    }
                );
            }
        }
        else if (interaction == "exit_bottom")
        {
            // exit_bottom_left
            if (Math.Abs(_avatar.Position.X - environment.Left) <
                Math.Abs(_avatar.Position.X - environment.Right)
            )
            {
                _avatar.Place(
                    _avatar.Position.X,
                    environment.Bottom - 1
                );
                _avatar.CancelAction();

                _avatar.Action(
                    "land",
                    1,
                    0,
                    0,
                    0
                );
                _avatar.Action(
                    "stand",
                    1,
                    0,
                    0,
                    0,
                    () =>
                    {
                        if (_avatar.Config.Behavior.TryGetValue(
                            "exit_bottom_left",
                            out var behavior
                        ))
                        {
                            RandomSelectorBehavior.Run(
                                _avatar,
                                behavior
                            );
                        }
                        else
                        {
                            RandomSelectorBehavior.Run(
                                _avatar,
                                new CharacterBehavior
                                {
                                    States = new List<string>
                                    {
                                        "stand",
                                        "walk_l",
                                        "walk_r",
                                        "sleep",
                                    },
                                    Data = new Dictionary<string, CharacterBehaviorState>
                                    {
                                        ["stand"] = new CharacterBehaviorState
                                        {
                                            Name = "stand",
                                            Repeat = 1,
                                            Prob = 0.4d,
                                        },
                                        ["sleep"] = new CharacterBehaviorState
                                        {
                                            Name = "sleep",
                                            Repeat = 1,
                                            RevM = 1,
                                            Prob = 0.1d,
                                        },
                                        ["walk_l"] = new CharacterBehaviorState
                                        {
                                            Name = "walk",
                                            Repeat = 1,
                                            Prob = 0.25d,
                                        },
                                        ["walk_r"] = new CharacterBehaviorState
                                        {
                                            Name = "walk",
                                            Repeat = 1,
                                            RevH = 1,
                                            Prob = 0.25d,
                                        },
                                    }
                                }
                            );
                        }
                    }
                );
            }
            // exit_bottom_right
            else
            {
                _avatar.Place(
                    _avatar.Position.X,
                    environment.Bottom - 1
                );
                _avatar.CancelAction();

                _avatar.Action(
                    "land",
                    1,
                    0,
                    0,
                    0
                );
                _avatar.Action(
                    "stand",
                    1,
                    0,
                    0,
                    0,
                    () =>
                    {
                        if (_avatar.Config.Behavior.TryGetValue(
                            "exit_bottom_right",
                            out var behavior
                        ))
                        {
                            RandomSelectorBehavior.Run(
                                _avatar,
                                behavior
                            );
                        }
                        else
                        {
                            RandomSelectorBehavior.Run(
                                _avatar,
                                new CharacterBehavior
                                {
                                    States = new List<string>
                                    {
                                        "stand",
                                        "walk_l",
                                        "walk_r",
                                        "sleep",
                                    },
                                    Data = new Dictionary<string, CharacterBehaviorState>
                                    {
                                        ["stand"] = new CharacterBehaviorState
                                        {
                                            Name = "stand",
                                            Repeat = 1,
                                            Prob = 0.4d,
                                        },
                                        ["sleep"] = new CharacterBehaviorState
                                        {
                                            Name = "sleep",
                                            Repeat = 1,
                                            Prob = 0.1d,
                                        },
                                        ["walk_l"] = new CharacterBehaviorState
                                        {
                                            Name = "walk",
                                            Repeat = 1,
                                            Prob = 0.25d,
                                        },
                                        ["walk_r"] = new CharacterBehaviorState
                                        {
                                            Name = "walk",
                                            Repeat = 1,
                                            RevH = 1,
                                            Prob = 0.25d,
                                        },
                                    }
                                }
                            );
                        }
                    }
                );
            }
        }

        return Task.CompletedTask;
    }
}
