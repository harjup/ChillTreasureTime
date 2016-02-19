using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Bird : MonoBehaviour, IExaminable
{
    public string TextId;
    private GuiCanvas _guiCanvas;
    private TalkingUi _talkingUi;
    private Animator _animator;
    private GameObject QMark;

    public List<Direction> Directions;

    private BirdProps _birdProps;

    public void Start()
    {
        _guiCanvas = GuiCanvas.Instance;
        _talkingUi = _guiCanvas.TalkingUi;

        _animator = GetComponentInChildren<Animator>();

        _birdProps = BirdPropsService.Instance.GetBirdPropsById(TextId);

        if (_birdProps != null)
        {
            if (_birdProps.AnimatorController != null)
            {
                _animator.runtimeAnimatorController = _birdProps.AnimatorController;
            }

            _animator.CrossFade(_birdProps.DefaultAnim, 0f);
        }
        else
        {
            _birdProps = new BirdProps("Idle", true, null);
        }

        QMark = transform.FindChild("?").gameObject;
        QMark.SetActive(false);

        Directions = DirectionService.Instance.GetDirectionsById(TextId);

        _animator.CrossFade(_birdProps.DefaultAnim, 0f);
        _animator.transform.localScale = 
                _animator.transform.localScale.SetX(_birdProps.FaceLeft ? 1 : -1);


        if (State.Instance.LevelEntrance == LevelEntrance.BeachRival && TextId == "Rival")
        {
            var lossDialog = new List<Direction>
            {
                new Line("Winson \"Collecto\"", "Woof. Erm, Chirp. Not the best dancing I've seen."),
                new Line("Winson \"Collecto\"", "We can go again any time. You may need to practice.")
            };

            var perfectDialog = new List<Direction>
            {
                new Line("Winson \"Collecto\"", "Wow. Just wow. I am very impressed."),
                new Line("Winson \"Collecto\"", "So, about your prize. It's the most prized and valuable thing to my name."),
                new Line("Winson \"Collecto\"", "It's... my respect. You have my respect. Good job guy."),
                new Line("Winson \"Collecto\"", "We can go again any time.")
            };

            var okDialog = new List<Direction>
            {
                new Line("Winson \"Collecto\"", "Pretty good. Not perfect, but pretty good."),
                new Line("Winson \"Collecto\"", "You're an ok bird."),
                new Line("Winson \"Collecto\"", "We can go again any time, if you'd like.")
            };


            Directions = new List<Direction>
            {
                new Line("Winson \"Collecto\"", "Wow. Just wow. I am very impressed."),
                new Line("Winson \"Collecto\"", "So, about your prize. It's the most prized and valuable thing to my name."),
                new Line("Winson \"Collecto\"", "It's... my respect. You have my respect. Good job guy."),
                new Line("Winson \"Collecto\"", "We can go again any time.")
            };

            if (State.Instance.FailedBeats <= 5 
                && State.Instance.FailedBeats > 2)
            {
                Directions = okDialog;
            }
            else if (State.Instance.FailedBeats <= 2)
            {
                Directions = perfectDialog;
            }
            else
            {
                Directions = lossDialog;
            }

            _animator.CrossFade("Talk", 0f);
            StartCoroutine(StartSequence(() =>
            {
                Directions = DirectionService.Instance.GetDirectionsById(TextId);
                FindObjectOfType<Control>().Disabled = false;
                _animator.CrossFade("Idle", 0f);
            }));
        }
    }


    public IEnumerator StartSequence(Action doneCallback)
    {
        QMark.SetActive(false);
        _guiCanvas.EnableTalking();

        foreach (var direction in Directions)
        {
            if (direction is Line)
            {
                _animator.CrossFade("Talk", 0f);
                yield return StartCoroutine(_talkingUi.TextCrawl(direction as Line, () =>
                {
                    _animator.CrossFade("Idle", 0f);
                }));
            }

            // Not currently used, won't get hit
            if (direction is GetWingFlap)
            {
                State.Instance.UnlockWingFlap();
            }

            if (direction is CameraFadeIn)
            {
                yield return SceneFadeInOut.Instance.StartScene();
            }

            if (direction is CameraFadeOut)
            {
                yield return SceneFadeInOut.Instance.EndScene();
            }

            if (direction is LoadDance)
            {
                doneCallback();

                _guiCanvas.DisableAll();

                SceneFadeInOut.Instance.EndScene();

                yield return new WaitForSeconds(.5f);

                LevelLoader.Instance.LoadDance();
            }

            if (direction is FightChoicePrompt)
            {
                _guiCanvas.EnableChoice();
                yield return new WaitForSeconds(.1f);
                var result = false;
                yield return StartCoroutine(_guiCanvas.ChoiceUi.WaitForPlayerChoice(res =>
                {
                    result = res;
                }));
                _guiCanvas.DisableChoice();

                if (result)
                {
                    _animator.CrossFade("Talk", 0f);
                    yield return StartCoroutine(_talkingUi.TextCrawl(
                        new Line("Winston \"Collecto\"", "Ok, cool."),
                        () =>
                        {
                            _animator.CrossFade("Idle", 0f);
                        }));

                    _animator.CrossFade("Talk", 0f);
                    yield return StartCoroutine(_talkingUi.TextCrawl(
                        new Line("Winston \"Collecto\"", "All you gotta do is watch what I do and copy my moves! Thinking about pressing an \"X\"ish button will make you peck the ground."),
                        () =>
                        {
                            _animator.CrossFade("Idle", 0f);
                        }));

                    _animator.CrossFade("Talk", 0f);
                    yield return StartCoroutine(_talkingUi.TextCrawl(
                        new Line("Winston \"Collecto\"", "If you're having a hard time, pay attention to when the helper arrow moves!"),
                        () =>
                        {
                            _animator.CrossFade("Idle", 0f);
                        }));

                    yield return SceneFadeInOut.Instance.EndScene();
                    Application.LoadLevel("Fight");
                }
                else
                {
                    _animator.CrossFade("Talk", 0f);
                    yield return StartCoroutine(_talkingUi.TextCrawl(new Line("Winston \"Collecto\"", "That's cool, I'm always open to get my groove on."), () => { _animator.CrossFade("Idle", 0f); }));
                }
            }

            if (direction is LeaveChoicePrompt)
            {
                _guiCanvas.EnableChoice();
                yield return new WaitForSeconds(.1f);
                var result = false;
                yield return StartCoroutine(_guiCanvas.ChoiceUi.WaitForPlayerChoice(res =>
                {
                    result = res;
                }));
                _guiCanvas.DisableChoice();

                if (result)
                {
                    _animator.CrossFade("Talk", 0f);
                    yield return StartCoroutine(_talkingUi.TextCrawl(new Line("Mayor Brachie", "Awesome. Let's go!"), () => { _animator.CrossFade("Idle", 0f); }));
                    yield return SceneFadeInOut.Instance.EndScene();
                    Application.LoadLevel("End");
                }
                else
                {
                    _animator.CrossFade("Talk", 0f);
                    yield return StartCoroutine(_talkingUi.TextCrawl(new Line("Mayor Brachie", "Alright, let me know."), () => { _animator.CrossFade("Idle", 0f); }));
                }
            }
        }

        _animator.CrossFade(_birdProps.DefaultAnim, 0f);
        doneCallback();
        _guiCanvas.EnableOverworldUi();
        QMark.SetActive(true);
    }

    public void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            QMark.SetActive(true);
            player.AddExaminable(this);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            QMark.SetActive(false);
            player.RemoveExaminable(this);
        }
    }
}
