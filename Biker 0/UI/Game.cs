using KCore;
using KCore.Extensions.InsteadSLThree;
using KCore.Graphics;
using KCore.Graphics.Containers;
using KCore.Graphics.Special;
using KCore.Graphics.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;

namespace Biker_0
{
    public class Game : Form
    {
        public Game(Player player)
        {
            CurrentPlayer = player;
            CurrentPlayer.Bike.UpdateCache(CurrentPlayer);

            GameWindow = new Window(
                GameScroll = new WithVerticalScroll(
                    GameText = new ScrollableText(),
                    new VerticalScroll(GameText, pixel: ' ', scrollPixel: '▐')
                ),
                title: "Игровые данные",
                container: new DynamicContainer()
                {
                    GetLeft = () => (Terminal.FixedWindowWidth - 50) / 2 - 15,
                    GetTop = () => (Terminal.FixedWindowHeight - 20) / 2,
                    GetWidth = () => 50,
                    GetHeight = () => 20,
                });
            SetTabTo(TabID_Main);

            ActionsWindow = new Window(
                ActionsList = new ListBox(),
                title: "Действия",
                container: new DynamicContainer()
                {
                    GetLeft = () => (Terminal.FixedWindowWidth - 50) / 2 + 34,
                    GetTop = () => (Terminal.FixedWindowHeight - 20) / 2 + 6,
                    GetWidth = () => 30,
                    GetHeight = () => 14,
                }
                );
            GetActions();

            Root.AddWidget(GameWindow);
            Root.AddWidget(GameScroll);
            Root.AddWidget(GameScroll.Scroll);
            Root.AddWidget(GameText);
            Root.AddWidget(ActionsWindow);
            Root.AddWidget(ActionsList);
            ActiveWidget = ActionsList;

            Root.AddWidget(console_text = new TextWidget(fillHeight: false));
            console_input = new TextInput(this);

            Bind(ActionTrigger = new Trigger(this, form => (form as Game).Action()));
            Bind(ScrollUp = new Trigger(this, form => (form as Game).GameText.ScrollUp()));
            Bind(ScrollDown = new Trigger(this, form => (form as Game).GameText.ScrollDown()));

        }

        public Trigger ActionTrigger;

        #region UI

        /*public class TabControl : Widget
        {
            public class Tab : Panel
            {
                public Tab(Form form) : base(form)
                {
                }
            }
        }*/

        #region Action
        public int[] ActionsRedirection;
        public const int ActionID_ShowMain = 0xE0;
        public const int ActionID_ShowBreaks = 0xE1;
        public const int ActionID_Exit = 0xFF;

        public const int ActionID_DBG_AddBreak = 0xDB_01;
        public const int ActionID_DBG_RemBreaks = 0xDB_02;
        public const int ActionID_DBG_Console = 0xDB_CC;
        public void GetActions()
        {
            var texts = ActionsList.Childs = new List<ListBox.ListItem>();
            var redirects = new List<int>();

            void RegAction(int redirect, string text)
            {
                texts.Add(new ListBox.ListItem(new TextWidget(text, fillHeight: false)));
                redirects.Add(redirect);
            }

            RegAction(ActionID_ShowMain, "Показать главное");
            RegAction(ActionID_ShowBreaks, "Показать повреждения");
#if DEBUG
            RegAction(ActionID_DBG_AddBreak, "[DBG] Добавить повреждение");
            RegAction(ActionID_DBG_RemBreaks, "[DBG] Удалить все повреждения");
            RegAction(ActionID_DBG_Console, "[DBG] Консоль");
#endif
            RegAction(ActionID_Exit, "Выйти");
            ActionsRedirection = redirects.ToArray();

        }

        private TextWidget console_text;
        private TextInput console_input;
        private SLThree.ExecutionContext slthree_context;
        public void Action()
        {
            var id = ActionsRedirection[ActionsList.Position];
            switch (id)
            {
                case ActionID_ShowMain:
                    {
                        SetTabTo(TabID_Main);
                    }
                    return;
                case ActionID_ShowBreaks:
                    {
                        SetTabTo(TabID_Breaks);
                    }
                    return;
                case ActionID_Exit:
                    {
                        Close();
                    }
                    return;
                case ActionID_DBG_Console:
                    {
                        if (console_input.InputMode)
                        {
                            var text = console_input.Deactivate();
                            try
                            {
                                if (slthree_context == null) slthree_context = new SLThree.ExecutionContext();
                                slthree_context.LocalVariables.SetValue("game", this);
                                var parsed = SLThree.Parser.This.ParseScript(text);
                                KCore.Tools.Log.Add("SLThree output: " + (parsed.GetValue(slthree_context)?.ToString() ?? "null"));
                            }
                            catch (Exception e)
                            {
                                KCore.Tools.Log.Add("SLThree error: " + e.ToString());
                            }
                            ActiveWidget = ActionsList;
                        }
                        else
                        {
                            ActiveWidget = console_input;
                            console_input.Activate();
                            console_input.OnAnyInput = input =>
                            {
                                console_text.Text = input.String.PadRight(Terminal.FixedWindowWidth - 1);
                                console_text.Resize();
                                console_text.Redraw();
                            };
                        }
                    }
                    return;
                case ActionID_DBG_AddBreak:
                    {
                        var rest_breaks = Data.Breaks.Select((x, i) => (x, i)).Where(x => !CurrentPlayer.Bike.BreaksIds.Contains(x.i)).Select(x => x.x).ToArray();
                        if (rest_breaks.Length == 0) return;
                        var chooser = new ChanceChooser<BikeBreak>(rest_breaks.ConvertAll(x => (x, x.Frequency)));
                        CurrentPlayer.Bike.BreaksIds.Add(Array.IndexOf(Data.Breaks, chooser.Choose()));
                        CurrentPlayer.Bike.UpdateCache(CurrentPlayer);
                    }
                    return;
                case ActionID_DBG_RemBreaks:
                    {
                        CurrentPlayer.Bike.BreaksIds.Clear();
                        CurrentPlayer.Bike.UpdateCache(CurrentPlayer);
                    }
                    return;
            }
        }
        #endregion

        public const int TabID_Main = 0;
        public const int TabID_Breaks = 1;

        public void SetTabTo(string text)
        {
            GameText.Text = text;
            GameWindow.Resize();
            GameText.Resize();
            GameScroll.Redraw();
            GameScroll.Scroll.Redraw();
            GameText.Redraw();
        }

        public bool SetTab(Action<StringBuilder> action)
        {
            var sb = new StringBuilder();
            action(sb);
            SetTabTo(sb.ToString());
            return true;
        }

        public bool SetTabTo(int index)
        {
            switch (index)
            {
                case TabID_Main: return SetTab(sb =>
                {
                    sb.AppendLine($"{CurrentPlayer.Town.Name}");
                    sb.AppendLine($"%=>DarkGreen%{CurrentPlayer.Money / 100.0}$%=>reset% наличных\n");
                    sb.AppendLine($"{CurrentPlayer.Bike.Name}");
                    sb.AppendLine($"Скорость: %=>DarkGray%{CurrentPlayer.Bike.AsphaltSpeed / 1000.0:0.##}%=>reset%/%=>DarkYellow%{CurrentPlayer.Bike.GroundSpeed / 1000.0:0.##}%=>reset% км/ч");
                    sb.AppendLine($"Ускорение: %=>DarkGray%{CurrentPlayer.Bike.AsphaltAcceleration / 1000.0:0.##}%=>reset%/%=>DarkYellow%{CurrentPlayer.Bike.GroundAcceleration / 1000.0:0.##}%=>reset%");
                    sb.AppendLine($"Трюки: %=>Magenta%{CurrentPlayer.Bike.TrickAcceleration / 1000.0:0.##}%=>reset%/%=>DarkCyan%{CurrentPlayer.Bike.TrickComfort / 1000.0:0.##}%=>reset%/%=>DarkRed%{CurrentPlayer.Bike.TrickBrake / 1000.0:0.##}%=>reset%");
                    var brcount = CurrentPlayer.Bike.BreaksIds.Count;
                    sb.AppendLine($"Повреждений: %=>{(brcount == 0 ? "DarkGreen" : "Red")}%{brcount}%=>reset%");
                });
                case TabID_Breaks: return SetTab(sb =>
                {
                    sb.AppendLine($"Цена полного ремонта: %=>DarkRed%{CurrentPlayer.Bike.Breaks.Sum(x => x.RepairPrice)/100.0}$%=>reset%\n");
                    foreach (var x in CurrentPlayer.Bike.Breaks)
                    {
                        var price = (x.RepairPrice / 100.0) + "$ ";
                        sb.AppendLine($"%=>DarkRed%{price}%=>reset%{x.Name.Reduce(46 - price.Length)}");
                        GetEffectInfoFor(x.Effect, sb);
                    }
                    sb.Append("-----");
                });
            }
            return false;
        }
        public void GetEffectInfoFor(object o, StringBuilder sb)
        {
            if (o is BikeSpeed.IBoost || o is BikeSpeed.IMultiplier)
            {
                var boosttext = "";
                if (o is BikeSpeed.IBoost boost)
                {
                    if (boost.BoostBikeAshpaltSpeed != 0)
                        boosttext += $"%=>DarkGray%{boost.BoostBikeAshpaltSpeed / 10 / 100.0}%=>reset% км/ч ";
                    if (boost.BoostBikeGroundSpeed != 0)
                        boosttext += $"%=>DarkYellow%{boost.BoostBikeGroundSpeed / 10 / 100.0}%=>reset% км/ч ";
                }
                var multipliertext = "";
                if (o is BikeSpeed.IMultiplier multiplier)
                {
                    if (multiplier.MultiplierBikeAsphaltSpeed != 1.0)
                        multipliertext += $"%=>DarkGray%{multiplier.MultiplierBikeAsphaltSpeed - 1.0:P0}%=>reset% ";
                    if (multiplier.MultiplierBikeGroundSpeed != 1.0)
                        multipliertext += $"%=>DarkYellow%{multiplier.MultiplierBikeGroundSpeed - 1.0:P0}%=>reset% ";
                }
                if (boosttext.Length > 0 || multipliertext.Length > 0)
                {
                    sb.Append($"Скорость: {boosttext}{multipliertext}");
                    sb.AppendLine();
                }
            }
            if (o is BikeAcceleration.IBoost || o is BikeAcceleration.IMultiplier)
            {
                var boosttext = "";
                if (o is BikeAcceleration.IBoost boost)
                {
                    if (boost.BoostBikeAsphaltAcceleration != 0)
                        boosttext += $"%=>DarkGray%{boost.BoostBikeAsphaltAcceleration / 10 / 100.0}%=>reset% ";
                    if (boost.BoostBikeGroundAcceleration != 0)
                        boosttext += $"%=>DarkYellow%{boost.BoostBikeGroundAcceleration / 10 / 100.0}%=>reset% ";
                }
                var multipliertext = "";
                if (o is BikeAcceleration.IMultiplier multiplier)
                {
                    if (multiplier.MultiplierBikeAsphaltAcceleration != 1.0)
                        multipliertext += $"%=>DarkGray%{multiplier.MultiplierBikeAsphaltAcceleration - 1.0:P0}%=>reset% ";
                    if (multiplier.MultiplierBikeGroundAcceleration != 1.0)
                        multipliertext += $"%=>DarkYellow%{multiplier.MultiplierBikeGroundAcceleration - 1.0:P0}%=>reset% ";
                }
                if (boosttext.Length > 0 || multipliertext.Length > 0)
                {
                    sb.Append($"Ускорение: {boosttext}{multipliertext}");
                    sb.AppendLine();
                }
            }
            if (o is BikeTrick.IBoost || o is BikeTrick.IMultiplier)
            {
                var boosttext = "";
                if (o is BikeTrick.IBoost boost)
                {
                    if (boost.BoostBikeTrickAcceleration != 0)
                        boosttext += $"%=>Magenta%{boost.BoostBikeTrickAcceleration / 10 / 100.0}%=>reset% ";
                    if (boost.BoostBikeTrickComfort != 0)
                        boosttext += $"%=>DarkCyan%{boost.BoostBikeTrickComfort / 10 / 100.0}%=>reset% ";
                    if (boost.BoostBikeTrickBrake != 0)
                        boosttext += $"%=>DarkRed%{boost.BoostBikeTrickBrake / 10 / 100.0}%=>reset% ";
                }
                var multipliertext = "";
                if (o is BikeTrick.IMultiplier multiplier)
                {
                    if (multiplier.MultiplierBikeTrickAcceleration != 1.0)
                        multipliertext += $"%=>Magenta%{multiplier.MultiplierBikeTrickAcceleration - 1.0:P0}%=>reset% ";
                    if (multiplier.MultiplierBikeTrickComfort != 1.0)
                        multipliertext += $"%=>DarkCyan%{multiplier.MultiplierBikeTrickComfort - 1.0:P0}%=>reset% ";
                    if (multiplier.MultiplierBikeTrickBrake != 1.0)
                        multipliertext += $"%=>DarkRed%{multiplier.MultiplierBikeTrickBrake - 1.0:P0}%=>reset% ";
                }
                if (boosttext.Length > 0 || multipliertext.Length > 0)
                {
                    sb.Append($"Трюки: {boosttext}{multipliertext}");
                    sb.AppendLine();
                }
            }
        }

        public Window ActionsWindow;
        public ListBox ActionsList;

        public Window GameWindow;
        public WithVerticalScroll GameScroll;
        public ScrollableText GameText;
        public Trigger ScrollUp;
        public Trigger ScrollDown;

        protected override void OnKeyDown(byte key)
        {
            if (console_input.InputMode)
            {
                if (key == Key.Enter)
                    ActionTrigger.Pull();
                console_input.OnKeyDown(key);
                return;
            }
            base.OnKeyDown(key);
            if (key == Key.E || key == Key.Enter)
                ActionTrigger.Pull();
            if (key == Key.F)
                ScrollUp.Send();
            if (key == Key.R)
                ScrollDown.Send();
        }

        #endregion

        #region Data
        public Player CurrentPlayer { get; set; }

        #endregion

    }
}
