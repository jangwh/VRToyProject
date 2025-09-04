using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using UnityEngine.SocialPlatforms.Impl;

public class Animal : MonoBehaviour
{
    Rigidbody rigid;
    Animator anim;
    CapsuleCollider capsuleCollider;
    public int level;
    public string animalName;

    public bool isDrag;
    bool isMerge;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    void Update()
    {
        if (isDrag)
        {
            transform.position = GameManager.Instance.animalGroup.position;
        }
    }
    public void Drag()
    {
        isDrag = true;
    }

    public void Drop()
    {
        isDrag = false;
        rigid.useGravity = true;
    }
    private void OnEnable()
    {
        switch(animalName)
        {
            case "Goldfish":
                level = 0;
                break;
            case "Chick":
                level = 1;
                break;
            case "Hen":
                level = 2;
                break;
            case "Rabbit":
                level = 3;
                break;
            case "Cat":
                level = 4;
                break;
            case "ReinDeer":
                level = 5;
                break;
            case "Buffalo":
                level = 6;
                break;
            case "SeaLion":
                level = 7;
                break;
            case "Elephant":
                level = 8;
                break;
        }
        anim.SetInteger("Level", level);
        capsuleCollider.enabled = true;
        rigid.useGravity = false;
        isMerge = false;
    }
    void OnCollisionStay(Collision collision)
    {
        // 충돌 상대편이 동물일 때
        if (collision.gameObject.tag == "Animal")
        {
            Animal other = collision.gameObject.GetComponent<Animal>();
            // 조건 비교 (같은 이름인지 + 지금 합쳐지는 중이 아닌지 + 만렙이 아닌지)
            
            if (animalName == other.animalName && !isMerge && !other.isMerge && level < 8)
            {
                // 나와 상대편 위치 가져오기
                float meX = transform.position.x;
                float meY = transform.position.y;
                float otherX = other.transform.position.x;
                float otherY = other.transform.position.y;

                // 내가 상대편보다 위에 있거나, 같은 높이에서 오른쪽에 있을 때
                if (meY < otherY || (meY == otherY && meX > otherX))
                {
                    other.Hide(transform.position);
                    // 비활성화
                    LeanPool.Despawn(other.transform);
                    LevelUp();
                    ObjectManager.Instance.SpawnAnimal(level, transform.position);
                    GameManager.Instance.score++;
                    LeanPool.Despawn(this);
                }
                else
                {
                    LevelUp();
                    Hide(transform.position);
                    ObjectManager.Instance.SpawnAnimal(level, transform.position);
                    GameManager.Instance.score++;
                    LeanPool.Despawn(this);
                }
            }
            if (!rigid.useGravity) rigid.useGravity = true;
        }
        if(collision.gameObject.tag == "Bowl")
        {
            if (!rigid.useGravity) rigid.useGravity = true;
        }
        if(collision.gameObject.tag == "GameOver")
        {
            GameManager.Instance.isOver = true;
        }
    }
    void LevelUp()
    {
        isMerge = true;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = Vector3.zero;
        int ranNum = Random.Range(4, 7);
        GameManager.Instance.audioSource.PlayOneShot(GameManager.Instance.audioClip[ranNum]);
        StartCoroutine("LevelUpRoutine");
    }

    IEnumerator LevelUpRoutine()
    {
        level++;
        yield return new WaitForSeconds(0.2f);
        // 최대 레벨 갱신
        //GameManager.Instance.maxLevel = Mathf.Max(level, GameManager.Instance.maxLevel);
        // 잠금 OFF
        isMerge = false;
    }

    public void Hide(Vector3 targetPos)
    {
        // 잠금 ON
        isMerge = true;
        // 물리 효과 OFF
        rigid.useGravity = false;
        // 충돌 OFF
        capsuleCollider.enabled = false;
        // 숨기기 코루틴 실행
        StartCoroutine("HideRoutine", targetPos);
    }

    IEnumerator HideRoutine(Vector3 targetPos)
    {
        int timeCount = 0;
        while (timeCount < 20)
        {
            timeCount++;
            // 상대가 있을 시
            if (targetPos != Vector3.up * 100)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
            }
            // 게임 오버 시
            else if (targetPos == Vector3.up * 100)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.2f);
            }
            yield return null;
        }
        // 잠금 OFF
        isMerge = false;
    }
    void OnDisable()
    {
        // 동물 속성 초기화
        level = 0;
        //deadTime = 0;

        // 동물 위치, 크기, 회전값 초기화
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // 동물 물리 초기화
        rigid.useGravity = false;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = Vector3.zero;
        capsuleCollider.enabled = false;
    }
}
