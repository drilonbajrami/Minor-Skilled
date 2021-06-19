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

	public BehaviorAllele(Dominance pDominance, float pPeerUtility, float pOpponentUtility, float pSocializingChance) : base(pDominance) 
	{
		_peerUtility = pPeerUtility;
		_opponentUtility = pOpponentUtility;
		_socializingChance = pSocializingChance;
	}

	public override BehaviorAllele GetCopy(float mutationFactor = 0, float mutationChance = 0)
	{
		BehaviorAllele copy = new BehaviorAllele(Dominance, _peerUtility, _opponentUtility, _socializingChance);
		copy.Mutate(mutationFactor, mutationChance);
		return copy;
	}

	public override void Mutate(float mutationFactor, float mutationChance)
	{
		if (mutationFactor == 0)
			return;

		if (Random.Range(0.0f, 100.0f) <= mutationChance)
		{
			float mutation = mutationFactor * 2.0f;
			_peerUtility += Random.Range(-mutation, mutation);
			_peerUtility = Mathf.Clamp(_peerUtility, -1.0f, 1.0f);
		}

		if (Random.Range(0.0f, 100.0f) <= mutationChance)
		{
			float mutation = mutationFactor * 2.0f;
			_opponentUtility += Random.Range(-mutation, mutation);
			_opponentUtility = Mathf.Clamp(_opponentUtility, -1.0f, 1.0f);
		}

		if (Random.Range(0.0f, 100.0f) <= mutationChance)
		{
			float mutation = mutationFactor * 100.0f;
			_socializingChance += Random.Range(-mutation, mutation);
			_socializingChance = Mathf.Clamp(_socializingChance, 0.0f, 100.0f);
		}
	}
}
