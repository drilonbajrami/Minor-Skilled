using UnityEngine;

[System.Serializable]
public class BehaviorAllele : Allele<BehaviorAllele>
{
	[SerializeField] private float _peerUtility;
	[SerializeField] private float _opponentUtility;
	[SerializeField] private float _socializingChance;

	public float PeerUtility => _peerUtility;
	public float OpponentUtility => _opponentUtility;
	public float SocializingChance => _socializingChance;

	[HideInInspector] public float OverallUtility { get { return _peerUtility - _opponentUtility + _socializingChance; } }

	public BehaviorAllele(float pPeerUtility, float pOpponentUtility, float pSocializingChance) : base() 
	{
		_peerUtility = pPeerUtility;
		_opponentUtility = pOpponentUtility;
		_socializingChance = pSocializingChance;
	}

	public override BehaviorAllele GetCopy(float mutationFactor, float mutationChance)
	{
		return new BehaviorAllele(_peerUtility, _opponentUtility, _socializingChance);	
	}

	public override void Mutate(float mutationFactor, float mutationChance)
	{
		return;
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
