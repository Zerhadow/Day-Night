using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    public enum Enemy {Melee,Ranged,Flying,Boss};
    public Enemy currentEnemy;
    // [SerializeField] Animator animator; 

    [Header("Customizable")]
    [SerializeField] float _lookSpeed = 5f;
    [SerializeField] float distance;
    [SerializeField] float radius;
    [SerializeField] bool range = false;
    [SerializeField] bool takingdamage = false;
    [SerializeField] LayerMask L_Player;
    [SerializeField] Transform _attackTransform;
    
    [Header("Audio")]
    // public AudioSource orcAttack;
    // public AudioSource rangeAttack;
    // public AudioSource flyingAttack;
    // public AudioSource bossAttack;
    // public AudioSource orcDeath;
    // public AudioSource rangeDeath;
    // public AudioSource flyingDeath;
    // public AudioSource bossDeath;

    [Header("State")]
    [SerializeField] bool enemyDead;

    [Header("Navigation")]
    GameObject player;
    NavMeshAgent agent;
    
    [Header("Ranged")]
    [SerializeField] GameObject _projectilePrefab;
    [SerializeField] GameObject _shotpoint;

    [Header("Attack and Movement Delays")]
    [SerializeField] bool _random;
    [SerializeField] bool delay;

    [Header("Flying")]
    [SerializeField] GameObject[] _flyEmpties = new GameObject[8];
    [SerializeField] GameObject _projectileGravityPrefab;
    [SerializeField] GameObject _smoke;
    
    // assigns player and accesses
    void Awake() {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    // calls populate enemy if enemy enum is flying
    void Start() {
        if(currentEnemy == Enemy.Flying) {
            PopulateFlyingArray();
        }

        if(currentEnemy == Enemy.Boss) {
            radius = 6.5f;
        }

        if(currentEnemy == Enemy.Melee) {
            radius = 1.25f;
        }
    }

    // Because most enemies follow player, we call that
    // every frame and differentiate enums later inside the function.
    // Ranged is the only enemy that runs from the player.
    void Update() {
        if(currentEnemy == Enemy.Melee || currentEnemy == Enemy.Boss){
            range = Physics.CheckSphere(_attackTransform.position, radius, L_Player);
        }

        FollowPlayer(currentEnemy);
        
        if(currentEnemy == Enemy.Ranged && !takingdamage) {
            RunFromPlayer();
        }
    }

    // This function finds and assigns all player 
    // empty transforms for the flying enemy to move to.
    void PopulateFlyingArray() {
        for(int i = 0; i < 8; i++) {
            _flyEmpties[i] = GameObject.Find("FlyTransform" + i);
        }
    }

    // takes enemy enum parameter and determines AI using passed enum.
    // Melee & Boss enemies walk towards player close,
    // ranged enemies start following at a further distance, and flying enemies trigger
    // RandomFlyingPosition to move towards the empties assigned in PopulateFlyingArray.
    private void FollowPlayer(Enemy enemy) {
        distance = Vector3.Distance(player.transform.position, this.transform.position);
        
        // if enemy type is melee, not dead, and 
        // distance is less than two meters, stop and attack
        if(enemy == Enemy.Melee) {
            if(distance < 5f) {
                if(takingdamage == false) {
                    // animator.SetBool("Idle",true);
                    // animator.SetBool("Walk",false);
                }

                LookAtPlayer();
            }

            if(distance < 4.2f || enemyDead) {
                agent.isStopped = true;
                //LookAtPlayer();
                if(enemyDead == false) {
                    if(delay == false && takingdamage == false)
                    StartCoroutine(MeleeAttack());
                }
            }

            //if enemy is not within 4 meters, navigate towards player
            if(distance > 4f && !takingdamage) {
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
                // animator.SetBool("Idle",false);
                // animator.SetBool("Walk",true);
            }
        }

        if(enemy == Enemy.Boss) {
            if(distance < 7f || enemyDead) {
                if(!takingdamage){
                    // animator.SetBool("Idle",true);
                    // animator.SetBool("Walk",false);
                }

                LookAtPlayer();
                agent.isStopped = true;

                if(enemyDead == false) {
                    if(delay == false) {
                        if(!takingdamage) {
                            StartCoroutine(BossAttack());
                        }
                    }
                }
            } else if(!takingdamage) { // if player is not within 7 meters, navigate towards player
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
                // animator.SetBool("Idle",false);
                // animator.SetBool("Walk",true);
            }
        }

        // if enemy type is ranged, alive and distance 
        // is greater than 17m, navigate towards the player 
        if(enemy == Enemy.Ranged) {
            if(distance > 17f && !enemyDead) {
                if(!takingdamage) {
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
                // animator.SetBool("Idle",false);
                // animator.SetBool("Walk",true);
                }
            }
        }
        
        if(enemy == Enemy.Flying) {
            LookAtPlayer();
            if(_random == false) {
                StartCoroutine(RandomFlyingPosition());
            }
        }
    }

    // Script used only by ranged enemy. Chooses a runaway position in the
    // opposite direction of player when enemy is too close.
    private void RunFromPlayer() {
        //if enemy alive and distance between 17-25m, stop and shoot
        if((distance >= 17f && distance <= 25f) && !enemyDead) {
            agent.isStopped = true;
            // animator.SetBool("Idle",true);
            // animator.SetBool("Walk",false);
            if(!enemyDead) LookAtPlayer();
            if(!delay) {
                StartCoroutine(RangedAttack());
            }

            //if enemy is dead stop moving
            if(enemyDead) {
                agent.isStopped = true;
            }
            
        } else if(distance < 17f ) { //if enemy is less than 17 meters, run away from player
            agent.isStopped = false;
            // animator.SetBool("Idle",false);
            // animator.SetBool("Walk",true);
            Vector3 directionToPlayer = transform.position - player.transform.position;
            Vector3 runAwayPos = transform.position + directionToPlayer;
            agent.SetDestination(runAwayPos);
        }
    }

    // Random flying position chooses one of eight empty player children from
    // flyEmpties[] and sets it as the AI/s destination at random intervals.
    // Also activates attack script periodically.
    IEnumerator RandomFlyingPosition() {
        _random = true;
        if(!takingdamage) {
            agent.SetDestination(_flyEmpties[Random.Range(0,_flyEmpties.Length-1)].transform.position);
            yield return new WaitForSeconds(Random.Range(2f,4f));
            
            StartCoroutine(FlyingAttack());
        }

        _random = false;
    }

    // LookAtPlayer is only really useful for making Melee player face enemy when
    // uper close. Otherwise, AI Navmesh component makes enemy face the way it walks
    // It is also called every frame for flying enemies 
    private void LookAtPlayer() {
        if(enemyDead == false){
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _lookSpeed * Time.deltaTime);
        }
    }

    // for the ranged enemy when the player is too close.
    private void LookAwayFromPlayer() {
        Vector3 awayDirection = player.transform.position + transform.position;
        awayDirection.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(awayDirection), _lookSpeed * Time.deltaTime);
    }

    // MeleeAttack deals damage to the player. Still needs a PhysicsOverlap bool so that
    // it does not do damage to the player if they can escape in time.
    IEnumerator MeleeAttack() {
        if(takingdamage == false) {
            delay = true;
            agent.isStopped = true;
            // animator.SetBool("Walk",false);
            // animator.SetBool("Idle",false);
            // animator.SetBool("Attack",true);

            if(range) {
                if(!enemyDead){
                    // orcAttack.Play();
                }
            }

            yield return new WaitForSeconds(0.4f);

            if(range) {
                if(!takingdamage) {
                    if(!enemyDead) {
                        // make player take damage
                    }
                }
            }

            yield return new WaitForSeconds(0.1f);
            // animator.SetBool("Attack",false);
            // animator.SetBool("Idle",true);
            yield return new WaitForSeconds(1f);
            delay = false;
        }
    }

    // RangedAttack instantiates a projectile randomly at the ranged enemies position.
    // It rotates the projectile in the direction of the player, while the projectile itself
    // controls its own veloctity. It uses delay to prevent repeated calling.
    IEnumerator RangedAttack() {
        delay = true;
        yield return new WaitForSeconds(1f);
        if(!takingdamage) {
            // animator.SetBool("Attack",true);

            if(enemyDead == false) {
                if(_projectilePrefab != null) { //check if the prefab is assigned so no errors are returned
                    // animator.SetBool("Idle",false);
                    // animator.SetBool("Walk",false);
                    // animator.SetBool("Attack",true);
                    // animator.Play("Attack");
                    yield return new WaitForSeconds(1f);

                    if(!enemyDead)
                        Instantiate(_projectilePrefab,_shotpoint.transform.position,transform.rotation);

                    // rangeAttack.Play();
                }

                yield return new WaitForSeconds(0.1f);
                // animator.SetBool("Attack",false);
                // animator.SetBool("Idle",true);
                yield return new WaitForSeconds(Random.Range(3f,6f));
                delay = false;
            }
        }
    }

    // FlyingAttack instantiates a projectile randomly with gravity using an assigned prefab.
    // The prefav handles its own trajectory.
    IEnumerator FlyingAttack() {
        if(!takingdamage) {
            delay = true;
            yield return new WaitForSeconds(Random.Range(4f,6f));

            if(enemyDead == false) {
                // animator.SetBool("Attack",true);
                if(_projectileGravityPrefab != null) { //check if the prefab is assigned so no errors are returned
                    yield return new WaitForSeconds(0.1f);
                    // animator.SetBool("Attack",false);
                    yield return new WaitForSeconds(0.9f);

                    if(!takingdamage) {
                        Instantiate(_projectileGravityPrefab,_shotpoint.transform.position,Quaternion.identity);
                        // flyingAttack.Play();
                    }
                }

                yield return new WaitForSeconds(Random.Range(3f,5f));
                delay = false;
            }
        }
    }

    //  BossAttack deals damage to the player. Still needs a PhysicsOverlap bool so that
    //  it does not do damage to the player if they can escape in time.
    IEnumerator BossAttack() {
        //if(takingdamage == false){
            delay = true;
            agent.isStopped = true;
            // animator.SetBool("Walk",false);
            // animator.SetBool("Idle",false);
            // animator.SetBool("Attack",true);
            yield return new WaitForSeconds(1.2f);
            // bossAttack.Play();

            if(range) {
                if(!enemyDead) {
                    // have player take damage
                }
            }
            //if(distance > 7f){
            // animator.SetBool("Walk",false);
            // animator.SetBool("Idle",true);
            // animator.SetBool("Attack",false);
            // animator.Play("Idle");
       // }
            yield return new WaitForSeconds(2.5f);
            agent.isStopped = false;
            yield return new WaitForSeconds(0.5f);
            delay = false;
    }

    public void enemyDamaged() {
        StartCoroutine(TakeDamage());
    }
    
    //playes corrresponding death animation and sound
    public void Death() {
        agent.updatePosition = false;
        enemyDead = true;
        agent.enabled = false;
        // animator.SetBool("Death",true);

        if(currentEnemy == Enemy.Melee) {
            // orcDeath.Play();
        }
        if(currentEnemy == Enemy.Ranged) {
            // rangeDeath.Play();
        }
        if(currentEnemy == Enemy.Boss) {
            // bossDeath.Play();
            // h.Play();
        }
        if(currentEnemy == Enemy.Flying) {
            // flyingDeath.Play();
        }

        agent.isStopped = true;
        StartCoroutine(OnDeath());
    }
    
    public IEnumerator TakeDamage() {
        takingdamage = true;
        //yield return new WaitForSeconds(0.1f);
        agent.speed = 0;
        // animator.SetBool("Hit",true);
        yield return new WaitForSeconds(0.2f);
        // animator.SetBool("Hit",false);
        yield return new WaitForSeconds(0.1f);
        
        if(currentEnemy == Enemy.Melee)
            agent.speed = 5f;

        if(currentEnemy == Enemy.Boss)
            agent.speed = 3f;

        if(currentEnemy == Enemy.Ranged)
            agent.speed = 4.5f;

        if(currentEnemy == Enemy.Flying)
            agent.speed = 9f;
        
        //yield return new WaitForSeconds(0.1f);
        // animator.SetBool("Hit",false);
        takingdamage = false;
    }

    IEnumerator OnDeath() {
        yield return new WaitForSeconds(2.9f);

        if(currentEnemy == Enemy.Boss)
            Instantiate(_smoke,new Vector3(transform.position.x,transform.position.y-2f,transform.position.z),Quaternion.identity);
        else if(currentEnemy == Enemy.Flying)
            Instantiate(_smoke,new Vector3(transform.position.x,transform.position.y-4f,transform.position.z),Quaternion.identity);
        else
            Instantiate(_smoke,transform.position,Quaternion.identity);
    }
}