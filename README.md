# testTask
for main gameplay scene -> select Scenes/Gameplay

/// <Gameplay Scene gameobjects>
//Camera with weapon -> holds main camera with weapoon script and particle system references
  
//Game Manager -> holds references to UI, Player settings, Enemies settings
  to add new enemy expand Enemies array, here we can reference available walking or flying enemy, choose enemy Speed and HP, define it's color and time that will be required before spawn of another unit that type
  Spawn Distance from player define distance of enemy spawn from player position
  
//Canvas is separated into 
  restart panel
  visible after player lost, with data of player score and last best result
  
  mission UI
  with current score result, 
  bar representing player's HP, 
  hpinfo image works as a feedback for a player when he is attacked
