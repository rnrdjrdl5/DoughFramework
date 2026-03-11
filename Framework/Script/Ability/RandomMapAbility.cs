using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// by Claude Code, Codex
public class RandomMapAbility : Ability
{
    public enum DifficultyLevel
    {
        Easy = 1,
        Normal = 2,
        Hard = 3,
        VeryHard = 4,
        Extreme = 5
    }
    
    [SerializeField] int mapWidth = 5;
    [SerializeField] int mapHeight = 5;
    [SerializeField] DifficultyLevel difficulty = DifficultyLevel.Easy;
    [SerializeField] int minStartEndManhattanDistance = 5;
    
    Room[,] roomGrid;
    Room startRoom;
    Room endRoom;
    List<Room> allRooms;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        GenerateMap();
    }
    
    public void GenerateMap()
    {
        CreateRoomGrid();
        GenerateConnectionsByDifficulty();
        EnsurePathExists();
        SetStartAndEndRooms();
    }
    
    void CreateRoomGrid()
    {
        roomGrid = new Room[mapWidth, mapHeight];
        allRooms = new List<Room>();
        
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                roomGrid[x, y] = new Room(x, y);
                allRooms.Add(roomGrid[x, y]);
            }
        }
    }
    
    void GenerateConnectionsByDifficulty()
    {
        switch (difficulty)
        {
            case DifficultyLevel.Easy:
                GenerateSimpleConnections();
                break;
            case DifficultyLevel.Normal:
                GenerateModerateConnections();
                break;
            case DifficultyLevel.Hard:
                GenerateComplexConnections();
                break;
            case DifficultyLevel.VeryHard:
                GenerateVeryComplexConnections();
                break;
            case DifficultyLevel.Extreme:
                GenerateExtremeConnections();
                break;
        }
    }
    
    void GenerateSimpleConnections()
    {
        // 난이도 1: 높은 연결 확률로 직선 경로 많이 생성
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Room currentRoom = roomGrid[x, y];
                
                // 동쪽 연결 (90% 확률)
                if (x + 1 < mapWidth && Random.value > 0.1f)
                {
                    ConnectRooms(currentRoom, roomGrid[x + 1, y], (int)Room.RoomDirection.East);
                }
                
                // 남쪽 연결 (90% 확률)
                if (y + 1 < mapHeight && Random.value > 0.1f)
                {
                    ConnectRooms(currentRoom, roomGrid[x, y + 1], (int)Room.RoomDirection.South);
                }
            }
        }
    }
    
    void GenerateModerateConnections()
    {
        // 난이도 2: 보통 연결 확률
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Room currentRoom = roomGrid[x, y];
                
                // 동쪽 연결 (70% 확률)
                if (x + 1 < mapWidth && Random.value > 0.3f)
                {
                    ConnectRooms(currentRoom, roomGrid[x + 1, y], (int)Room.RoomDirection.East);
                }
                
                // 남쪽 연결 (70% 확률)
                if (y + 1 < mapHeight && Random.value > 0.3f)
                {
                    ConnectRooms(currentRoom, roomGrid[x, y + 1], (int)Room.RoomDirection.South);
                }
            }
        }
        
        // 일부 막다른 길 생성
        CreateDeadEnds(2);
    }
    
    void GenerateComplexConnections()
    {
        // 난이도 3: 낮은 연결 확률로 우회로 강제
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Room currentRoom = roomGrid[x, y];
                
                // 동쪽 연결 (50% 확률)
                if (x + 1 < mapWidth && Random.value > 0.5f)
                {
                    ConnectRooms(currentRoom, roomGrid[x + 1, y], (int)Room.RoomDirection.East);
                }
                
                // 남쪽 연결 (50% 확률)
                if (y + 1 < mapHeight && Random.value > 0.5f)
                {
                    ConnectRooms(currentRoom, roomGrid[x, y + 1], (int)Room.RoomDirection.South);
                }
            }
        }
        
        CreateDeadEnds(4);
        CreateAdditionalConnections(allRooms.Count / 5);
    }
    
    void GenerateVeryComplexConnections()
    {
        // 난이도 4: 매우 낮은 연결 확률
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Room currentRoom = roomGrid[x, y];
                
                // 동쪽 연결 (30% 확률)
                if (x + 1 < mapWidth && Random.value > 0.7f)
                {
                    ConnectRooms(currentRoom, roomGrid[x + 1, y], (int)Room.RoomDirection.East);
                }
                
                // 남쪽 연결 (30% 확률)
                if (y + 1 < mapHeight && Random.value > 0.7f)
                {
                    ConnectRooms(currentRoom, roomGrid[x, y + 1], (int)Room.RoomDirection.South);
                }
            }
        }
        
        CreateDeadEnds(6);
        CreateFalseEnds(2);
        CreateAdditionalConnections(allRooms.Count / 4);
    }
    
    void GenerateExtremeConnections()
    {
        // 난이도 5: 극도로 낮은 연결 확률
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Room currentRoom = roomGrid[x, y];
                
                // 동쪽 연결 (20% 확률)
                if (x + 1 < mapWidth && Random.value > 0.8f)
                {
                    ConnectRooms(currentRoom, roomGrid[x + 1, y], (int)Room.RoomDirection.East);
                }
                
                // 남쪽 연결 (20% 확률)
                if (y + 1 < mapHeight && Random.value > 0.8f)
                {
                    ConnectRooms(currentRoom, roomGrid[x, y + 1], (int)Room.RoomDirection.South);
                }
            }
        }
        
        CreateDeadEnds(10);
        CreateFalseEnds(4);
        CreateLoops(3);
        CreateAdditionalConnections(allRooms.Count / 3);
    }
    
    void ConnectRooms(Room room1, Room room2, int direction)
    {
        room1.SetConnection(direction, room2);
        room2.SetConnection(GetOppositeDirection(direction), room1);
    }
    
    void CreateAdditionalConnections(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Room randomRoom = allRooms[Random.Range(0, allRooms.Count)];
            CreateRandomConnection(randomRoom);
        }
    }
    
    void EnsurePathExists()
    {
        // 각 방이 최소 한 곳은 갈 수 있도록 보장
        foreach (Room room in allRooms)
        {
            if (room.GetAvailableDirections().Count == 0)
            {
                CreateRandomConnection(room);
            }
        }
        
        // 시작점에서 모든 방에 도달 가능하도록 보장
        EnsureAllRoomsReachable();
    }
    
    void CreateRandomConnection(Room room)
    {
        var possibleDirections = new List<int>();
        
        // 동쪽
        if (room.x + 1 < mapWidth)
            possibleDirections.Add((int)Room.RoomDirection.East);
        // 서쪽  
        if (room.x - 1 >= 0)
            possibleDirections.Add((int)Room.RoomDirection.West);
        // 남쪽
        if (room.y + 1 < mapHeight)
            possibleDirections.Add((int)Room.RoomDirection.South);
        // 북쪽
        if (room.y - 1 >= 0)
            possibleDirections.Add((int)Room.RoomDirection.North);
        
        if (possibleDirections.Count > 0)
        {
            int randomDir = possibleDirections[Random.Range(0, possibleDirections.Count)];
            Room targetRoom = GetRoomInDirection(room, randomDir);
            
            if (targetRoom != null)
            {
                room.SetConnection(randomDir, targetRoom);
                targetRoom.SetConnection(GetOppositeDirection(randomDir), room);
            }
        }
    }
    
    Room GetRoomInDirection(Room room, int direction)
    {
        int newX = room.x;
        int newY = room.y;
        
        switch (direction)
        {
            case (int)Room.RoomDirection.East: newX++; break;
            case (int)Room.RoomDirection.West: newX--; break;
            case (int)Room.RoomDirection.South: newY++; break;
            case (int)Room.RoomDirection.North: newY--; break;
        }
        
        if (newX >= 0 && newX < mapWidth && newY >= 0 && newY < mapHeight)
            return roomGrid[newX, newY];
        
        return null;
    }
    
    int GetOppositeDirection(int direction)
    {
        switch (direction)
        {
            case (int)Room.RoomDirection.East: return (int)Room.RoomDirection.West;
            case (int)Room.RoomDirection.West: return (int)Room.RoomDirection.East;
            case (int)Room.RoomDirection.South: return (int)Room.RoomDirection.North;
            case (int)Room.RoomDirection.North: return (int)Room.RoomDirection.South;
            default: return -1;
        }
    }
    
    void EnsureAllRoomsReachable()
    {
        var visited = new HashSet<Room>();
        var queue = new Queue<Room>();
        
        // 시작점에서 BFS
        Room startPoint = roomGrid[0, 0];
        queue.Enqueue(startPoint);
        visited.Add(startPoint);
        
        while (queue.Count > 0)
        {
            Room current = queue.Dequeue();
            
            for (int dir = 0; dir < (int)Room.RoomDirection.Max; dir++)
            {
                Room neighbor = current.GetConnection(dir);
                if (neighbor != null && !visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }
        
        // 도달할 수 없는 방들을 연결
        foreach (Room room in allRooms)
        {
            if (!visited.Contains(room))
            {
                ConnectToReachableRoom(room, visited);
            }
        }
    }
    
    void ConnectToReachableRoom(Room isolatedRoom, HashSet<Room> reachableRooms)
    {
        // 가장 가까운 도달 가능한 방과 연결
        Room closestRoom = null;
        float minDistance = float.MaxValue;
        
        foreach (Room reachableRoom in reachableRooms)
        {
            float distance = Mathf.Abs(isolatedRoom.x - reachableRoom.x) + 
                           Mathf.Abs(isolatedRoom.y - reachableRoom.y);
            
            if (distance < minDistance && AreAdjacent(isolatedRoom, reachableRoom))
            {
                minDistance = distance;
                closestRoom = reachableRoom;
            }
        }
        
        if (closestRoom != null)
        {
            int direction = GetDirectionBetween(isolatedRoom, closestRoom);
            if (direction != -1)
            {
                isolatedRoom.SetConnection(direction, closestRoom);
                closestRoom.SetConnection(GetOppositeDirection(direction), isolatedRoom);
            }
        }
    }
    
    bool AreAdjacent(Room room1, Room room2)
    {
        int deltaX = Mathf.Abs(room1.x - room2.x);
        int deltaY = Mathf.Abs(room1.y - room2.y);
        return (deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1);
    }
    
    int GetDirectionBetween(Room from, Room to)
    {
        if (to.x > from.x) return (int)Room.RoomDirection.East;
        if (to.x < from.x) return (int)Room.RoomDirection.West;
        if (to.y > from.y) return (int)Room.RoomDirection.South;
        if (to.y < from.y) return (int)Room.RoomDirection.North;
        return -1;
    }
    
    int GetManhattanDistance(Room a, Room b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
    
    void SetStartAndEndRooms()
    {
        // 가장자리 방들 수집
        var edgeRooms = GetEdgeRooms();

        // 랜덤 시작점 선택
        startRoom = edgeRooms[Random.Range(0, edgeRooms.Count)];
        startRoom.isStartRoom = true;

        // 최소 맨해튼 거리 조건을 만족하는 도착점 후보 선택
        var candidateEndRooms = edgeRooms
            .Where(room => room != startRoom && GetManhattanDistance(startRoom, room) >= minStartEndManhattanDistance)
            .ToList();

        if (candidateEndRooms.Count > 0)
        {
            endRoom = candidateEndRooms[Random.Range(0, candidateEndRooms.Count)];
        }
        else
        {
            // 조건 만족 후보가 없으면 가장 멀리 떨어진 방으로 대체하고 경고 출력
            var availableEndRooms = edgeRooms.Where(room => room != startRoom).ToList();
            endRoom = availableEndRooms
                .OrderByDescending(room => GetManhattanDistance(startRoom, room))
                .FirstOrDefault();

            if (endRoom != null)
            {
                int dist = GetManhattanDistance(startRoom, endRoom);
                if (dist < minStartEndManhattanDistance)
                {
                    Debug.LogWarning($"[RandomMapAbility] minStartEndManhattanDistance={minStartEndManhattanDistance}를 만족하는 가장자리 방이 없습니다. 현재 맵 크기 {mapWidth}x{mapHeight}, 최대 가능 거리 {dist}로 대체합니다.");
                }
            }
            else
            {
                // 안전장치: 시작점과 다른 방이 없을 리 없지만, 혹시 모를 경우 임의 선택
                endRoom = edgeRooms[Random.Range(0, edgeRooms.Count)];
            }
        }

        endRoom.isEndRoom = true;
    }
    
    List<Room> GetEdgeRooms()
    {
        var edgeRooms = new List<Room>();
        
        // 상단 가장자리 (y = 0)
        for (int x = 0; x < mapWidth; x++)
            edgeRooms.Add(roomGrid[x, 0]);
            
        // 하단 가장자리 (y = mapHeight - 1)
        for (int x = 0; x < mapWidth; x++)
            edgeRooms.Add(roomGrid[x, mapHeight - 1]);
            
        // 좌측 가장자리 (x = 0, 모서리 제외)
        for (int y = 1; y < mapHeight - 1; y++)
            edgeRooms.Add(roomGrid[0, y]);
            
        // 우측 가장자리 (x = mapWidth - 1, 모서리 제외)
        for (int y = 1; y < mapHeight - 1; y++)
            edgeRooms.Add(roomGrid[mapWidth - 1, y]);
            
        return edgeRooms;
    }
    
    
    void CreateDeadEnds(int count)
    {
        // 막다른 길 생성
        for (int i = 0; i < count; i++)
        {
            Room randomRoom = allRooms[Random.Range(0, allRooms.Count)];
            if (randomRoom != startRoom && randomRoom != endRoom)
            {
                // 한 방향만 남기고 모든 연결 제거
                var availableDirections = randomRoom.GetAvailableDirections();
                if (availableDirections.Count > 1)
                {
                    int keepDirection = availableDirections[0];
                    for (int dir = 0; dir < (int)Room.RoomDirection.Max; dir++)
                    {
                        if (dir != keepDirection)
                        {
                            Room connectedRoom = randomRoom.GetConnection(dir);
                            if (connectedRoom != null)
                            {
                                randomRoom.SetConnection(dir, null);
                                connectedRoom.SetConnection(GetOppositeDirection(dir), null);
                            }
                        }
                    }
                }
            }
        }
    }
    
    void CreateFalseEnds(int count)
    {
        // 가짜 목적지 생성 (막다른 길이지만 목적지처럼 보이는 곳)
        var edgeRooms = GetEdgeRooms().Where(room => room != startRoom && room != endRoom).ToList();
        
        for (int i = 0; i < count && i < edgeRooms.Count; i++)
        {
            Room falseEnd = edgeRooms[Random.Range(0, edgeRooms.Count)];
            edgeRooms.Remove(falseEnd);
            
            // 이 방을 막다른 길로 만들기
            var availableDirections = falseEnd.GetAvailableDirections();
            if (availableDirections.Count > 1)
            {
                int keepDirection = availableDirections[0];
                for (int dir = 0; dir < (int)Room.RoomDirection.Max; dir++)
                {
                    if (dir != keepDirection)
                    {
                        Room connectedRoom = falseEnd.GetConnection(dir);
                        if (connectedRoom != null)
                        {
                            falseEnd.SetConnection(dir, null);
                            connectedRoom.SetConnection(GetOppositeDirection(dir), null);
                        }
                    }
                }
            }
        }
    }
    
    void CreateLoops(int count)
    {
        // 순환 경로 생성
        for (int i = 0; i < count; i++)
        {
            Room randomRoom1 = allRooms[Random.Range(0, allRooms.Count)];
            Room randomRoom2 = allRooms[Random.Range(0, allRooms.Count)];
            
            if (randomRoom1 != randomRoom2 && AreAdjacent(randomRoom1, randomRoom2))
            {
                int direction = GetDirectionBetween(randomRoom1, randomRoom2);
                if (direction != -1 && randomRoom1.GetConnection(direction) == null)
                {
                    randomRoom1.SetConnection(direction, randomRoom2);
                    randomRoom2.SetConnection(GetOppositeDirection(direction), randomRoom1);
                }
            }
        }
    }
    
    // 공개 API
    public Room GetStartRoom() => startRoom;
    public Room GetEndRoom() => endRoom;
    public Room GetRoom(int x, int y) => roomGrid[x, y];
    public Room[,] GetRoomGrid() => roomGrid;
    
    public void SetDifficulty(DifficultyLevel newDifficulty)
    {
        difficulty = newDifficulty;
    }
    
    public void SetMapSize(int width, int height)
    {
        mapWidth = width;
        mapHeight = height;
    }

    // Debug Helpers

    // Finds a path from startRoom to endRoom via BFS and returns the visited route.
    // - Returns a list of Rooms representing the path (empty if not found).
    // - Populates moveDirections with the sequence of directions taken between rooms
    //   using Room.RoomDirection values (0:East,1:West,2:South,3:North).
    // - If includeEndpoints is true, the returned path contains both start and end.
    public List<Room> FindPathStartToEnd(out List<int> moveDirections, bool includeEndpoints = true)
    {
        moveDirections = new List<int>();

        if (startRoom == null || endRoom == null)
        {
            Debug.LogWarning("[RandomMapTrait] FindPathStartToEnd: startRoom or endRoom is null");
            return new List<Room>();
        }

        if (startRoom == endRoom)
        {
            var single = new List<Room> { startRoom };
            return single;
        }

        var queue = new Queue<Room>();
        var visited = new HashSet<Room>();
        var cameFrom = new Dictionary<Room, Room>();

        queue.Enqueue(startRoom);
        visited.Add(startRoom);

        bool found = false;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current == endRoom)
            {
                found = true;
                break;
            }

            for (int dir = 0; dir < (int)Room.RoomDirection.Max; dir++)
            {
                var next = current.GetConnection(dir);
                if (next != null && !visited.Contains(next))
                {
                    visited.Add(next);
                    cameFrom[next] = current;
                    queue.Enqueue(next);
                }
            }
        }

        if (!found)
        {
            // No route found
            return new List<Room>();
        }

        // Reconstruct path from end to start
        var path = new List<Room>();
        var cursor = endRoom;
        path.Add(cursor);
        while (cursor != startRoom)
        {
            cursor = cameFrom[cursor];
            path.Add(cursor);
        }
        path.Reverse();

        // Build move directions between consecutive rooms
        moveDirections.Clear();
        for (int i = 0; i < path.Count - 1; i++)
        {
            int dir = GetDirectionBetween(path[i], path[i + 1]);
            moveDirections.Add(dir);
        }

        if (!includeEndpoints)
        {
            // Return only the intermediate rooms (the actual trail), excluding start/end
            if (path.Count >= 2)
            {
                path = path.GetRange(1, path.Count - 2);
            }
            else
            {
                path.Clear();
                moveDirections.Clear();
            }
        }

        return path;
    }
}

[System.Serializable]
public class Room
{
    public enum RoomDirection
    {
        East = 0,
        West = 1,
        South = 2,
        North = 3,
        Max = 4
    }
    
    public int x, y;
    public List<Room> directions;
    public bool isStartRoom;
    public bool isEndRoom;
        
    public Room(int x, int y)
    {
        this.x = x;
        this.y = y;
        
        directions = new List<Room>(new Room[(int)RoomDirection.Max]);
        isStartRoom = false;
        isEndRoom = false;
    }
    
    public void SetConnection(int direction, Room room)
    {
        if (direction >= 0 && direction < (int)RoomDirection.Max)
            directions[direction] = room;
    }
        
    public Room GetConnection(int direction)
    {
        if (direction >= 0 && direction < (int)RoomDirection.Max)
            return directions[direction];
        return null;
    }
        
    public bool CanMoveTo(int direction)
    {
        return GetConnection(direction) != null;
    }
        
    public List<int> GetAvailableDirections()
    {
        var available = new List<int>();
        for (int i = 0; i < (int)RoomDirection.Max; i++)
        {
            if (CanMoveTo(i))
                available.Add(i);
        }
        return available;
    }
}
