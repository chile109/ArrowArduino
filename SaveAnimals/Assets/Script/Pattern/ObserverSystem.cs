using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface ObDataserver
{
    void BeNotified(string AniName, AnimalState state);
    void BeHit(int Horizental, int Vertical);
}


public class ObserverSystem : MonoBehaviour
{


    private static ObserverSystem _instants;
    public static ObserverSystem share { get { return _instants; } }

    private HashSet<ObDataserver> m_ObDateServers = new HashSet<ObDataserver>();

    void Awake()
    {
        if (_instants == null)
        {
            _instants = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        Notify("All", AnimalState.Idle);
    }

    public void Attach(ObDataserver theObserver)
    {
        if (theObserver != null)
            m_ObDateServers.Add(theObserver);

    }

    public void Detach(ObDataserver theObserver)
    {
        if (theObserver != null)
            m_ObDateServers.Remove(theObserver);
    }

    public void Notify(string AniName, AnimalState state)
    {
        //Debug.Log(AniName + ": " + state);
        foreach (var theObserver in m_ObDateServers)
            Task.Run(() => theObserver.BeNotified(AniName, state));
    }

    public void HitNotify(int _hori, int _verti)
    {
        //Debug.Log(AniName + ": " + state);
        foreach (var theObserver in m_ObDateServers)
            Task.Run(() => theObserver.BeHit(_hori, _verti));

    }
}
