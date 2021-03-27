using UnityEngine;

namespace AppleBoy
{
    [RequireComponent(typeof(SpriteAnimation))]
    public class DestroyAfterAnimation : MonoBehaviour
    {
        public string m_animation;
        private SpriteAnimation m_animator;

        public void Start()
        {
            m_animator = GetComponent<SpriteAnimation>();
            m_animator.PlayWithCallBack(m_animation, () =>
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            });
        }
    }
}
