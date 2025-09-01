using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    Rigidbody rigid;
    Animator anim;
    CapsuleCollider capsuleCollider;
    public int level;

    bool isDrag;
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
        anim.SetInteger("Level", level);

        if (!anim.GetBool("IsPlaying"))
        {
            anim.SetBool("IsPlaying", false);
            StartCoroutine(ResetAnimation());
        }
    }
    IEnumerator ResetAnimation()
    {
        yield return new WaitForSeconds(0.2f); // 애니메이션 길이에 맞춰 조정
        anim.SetBool("IsPlaying", true);  // 다시 실행되지 않도록 설정
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 충돌 상대편이 동물일 때
        if (collision.gameObject.tag == "Animal")
        {
            Animal other = collision.gameObject.GetComponent<Animal>();
            // 조건 비교 (같은 레벨인지 + 지금 합쳐지는 중이 아닌지 + 만렙이 아닌지)
            if (level == other.level && !isMerge && !other.isMerge && level < 8)
            {
                // 나와 상대편 위치 가져오기
                float meX = transform.position.x;
                float meY = transform.position.y;
                float otherX = other.transform.position.x;
                float otherY = other.transform.position.y;

                // 내가 상대편보다 위에 있거나, 같은 높이에서 오른쪽에 있을 때
                if (meY < otherY || (meY == otherY && meX > otherX))
                {
                    other.Hide(this.transform.position);
                    LevelUp();
                }
            }
        }
    }
    void LevelUp()
    {
        isMerge = true;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = Vector3.zero;

        StartCoroutine("LevelUpRoutine");
    }

    IEnumerator LevelUpRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("Level", level + 1);
        anim.SetBool("IsPlaying", false);
        StartCoroutine(ResetAnimation());

        yield return new WaitForSeconds(0.3f);
        level++;
        // 최대 레벨 갱신
        GameManager.Instance.maxLevel = Mathf.Max(GameManager.Instance.maxLevel, level);
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
        // 비활성화
        gameObject.SetActive(false);
        // 잠금 OFF
        isMerge = false;
    }
    void OnDisable()
    {
        // 동글 속성 초기화
        level = 0;
        //deadTime = 0;

        // 동글 위치, 크기, 회전값 초기화
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // 동글 물리 초기화
        rigid.useGravity = false;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = Vector3.zero;
        capsuleCollider.enabled = true;
    }
}
