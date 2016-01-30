using UnityEngine;
using System.Collections;

public class FloatingObject : MonoBehaviour {
	
	public	enum EDirection 
	{
		positif	= 1,
		negatif	= -1
	}
	public	EDirection	direcTranslation;
	public	EDirection	direcRotation;
	public	float		timeTranslation		= 1.0f;
	public	float		timeRotation		= 1.0f;
	public	float		lengthTranslation	= 1.0f;
	public	float		angleRotation		= 30.0f;
	
	private	Vector3		originePos;
	private	Vector3		origineRot;
	private	Transform	transEntity;
	private	Vector3		endPos;
	private	Vector3		endRot;
	private	bool		sensPos;
	private	bool		sensRot;
	private	float		actualPos;
	private	float		actualRot;
	private	float		timeBegPos;
	private	float		timeBegRot;
	
	// Use this for initialization
	void Awake () {
		this.transEntity	= transform;
		this.originePos		= this.transEntity.position;
		this.origineRot		= this.transEntity.rotation.eulerAngles;
		this.endPos			= this.originePos + new Vector3(0, (int)this.direcTranslation * this.lengthTranslation, 0);
		this.endRot			= this.origineRot + new Vector3(0, (int)this.direcRotation * this.angleRotation, 0);
	}
	
	// Update is called once per frame
	void Update () {
		move ();
	}
	
	void	move()
	{
		if (this.timeBegPos == 0f)
			this.timeBegPos	= Time.time;
		
		if (this.timeBegRot == 0f)
			this.timeBegRot	= Time.time;
		
		this.actualPos	= (Time.time - this.timeBegPos) / this.timeTranslation;
		if (actualPos > 1f)
			actualPos	= 1f;
		
		this.actualRot	= (Time.time - this.timeBegRot) / this.timeRotation;
		if (actualRot > 1f)
			actualRot	= 1f;
		
		if (sensPos)
			transEntity.position	= Vector3.Lerp(this.originePos, this.endPos, Mathf.SmoothStep(0, 1, this.actualPos));
		else
			transEntity.position	= Vector3.Lerp(this.endPos, this.originePos, Mathf.SmoothStep(0, 1, this.actualPos));
		
		if (sensRot)
			transEntity.rotation	= Quaternion.Euler(Vector3.Lerp(this.origineRot, this.endRot, Mathf.SmoothStep(0, 1, this.actualRot)));
		else
			transEntity.rotation	= Quaternion.Euler(Vector3.Lerp(this.endRot, this.origineRot, Mathf.SmoothStep(0, 1, this.actualRot)));
		
		if (actualPos == 1f)
		{
			this.timeBegPos	= Time.time;
			sensPos			= !sensPos;
		}
		
		if (actualRot == 1f)
		{
			this.timeBegRot	= Time.time;
			sensRot			= !sensRot;
		}
	}
}
