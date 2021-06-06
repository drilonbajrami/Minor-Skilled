using UnityEngine;

[System.Serializable]
public class BehaviorAllele : Allele<BehaviorAllele>
{
	[Range(-1, 1)]  [SerializeField] private float _peerUtility;
	[Range(-1, 1)]  [SerializeField] private float _opponentUtility;
	[Range(0, 100)] [SerializeField] private float _socializingChance;

	public float PeerUtility       => _peerUtility;
	public float OpponentUtility   => _opponentUtility;
	public float SocializingChance => _socializingChance;

	[HideInInspector] public float OverallUtility { get { return _peerUtility - _opponentUtility + _socializingChance; } }

	public BehaviorAllele(float pPeerUtility, float pOpponentUtility, float pSocializingChance) : base() 
	{
		_peerUtility = pPeerUtility;
		_opponentUtility = pOpponentUtility;
		_socializingChance = pSocializingChance;
	}

	public override BehaviorAllele GetCopy(float mutationFactor = 0, float mutationChance = 0)
	{
		return new BehaviorAllele(_peerUtility, _opponentUtility, _socializingChance);	
	}

	public void ChangePeerUtility(float change)
	{
		_peerUtility += change;
		_peerUtility = Mathf.Clamp(_peerUtility, -1.0f, 1.0f);
	}

	public void ChangeOpponentUtility(float change)
	{
		_opponentUtility += change;
		_opponentUtility = Mathf.Clamp(_opponentUtility, -1.0f, 1.0f);
	}

	public void ChangeSocializingChance(float change)
	{
		_socializingChance += change;
		_socializingChance = Mathf.Clamp(_socializingChance, 0.0f, 100.0f);
	}
}
