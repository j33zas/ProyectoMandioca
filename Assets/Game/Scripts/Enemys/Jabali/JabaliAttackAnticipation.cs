using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class JabaliAttackAnticipation : JabaliStates
    {
        float anticipationTime;
        float timer = 0;

        public JabaliAttackAnticipation(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float antTime) : base(myState, _sm)
        {
            anticipationTime = antTime;
        }

        protected override void Enter(JabaliEnemy.JabaliInputs input)
        {
            if (input != JabaliEnemy.JabaliInputs.PETRIFIED)
            {
                //setear booleano de la animación
                combatDirector.RemoveToAttack(enemy, enemy.CurrentTarget());
            }
        }

        protected override void Update()
        {
            Vector3 myForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
            Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);

            Root(forwardRotation);

            timer += Time.deltaTime;

            if (timer >= anticipationTime)
            {
                sm.SendInput(JabaliEnemy.JabaliInputs.HEAD_ATTACK);

            }
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            //setear animación para que no cause problemas
            timer = 0;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
        }
    }
}
