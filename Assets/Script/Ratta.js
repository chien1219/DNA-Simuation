#pragma strict

function Start () {

}


function Update(){
      transform.RotateAround(Vector3(-10,3,0), Vector3.up, 40 * Time.deltaTime);
}