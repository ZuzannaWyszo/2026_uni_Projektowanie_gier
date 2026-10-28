using UnityEngine;

public class PipesGenerator : BasePuzzle
{
    public int width = 7;
    public int height = 7;
    public float tileSize = 0.42f; 
    
    public GameObject prefabEnd;
    public GameObject prefabStraight;
    public GameObject prefabCorner;
    public GameObject prefabT;

    private int[,] gridConnections; 
    private bool[,] visited;
    public PipePiece[,] gridPipes;

    void Start()
    {
        GeneratePuzzle();
    }

    public void GeneratePuzzle()
    {
        gridConnections = new int[width, height];
        visited = new bool[width, height];

        BuildMaze(0, 0);

        SpawnTiles();
    }

    void BuildMaze(int x, int y)
    {
        visited[x, y] = true;
        int[] dirs = { 0, 1, 2, 3 };

        for (int i = 0; i < dirs.Length; i++)
        {
            int rnd = Random.Range(i, dirs.Length);
            int temp = dirs[i];
            dirs[i] = dirs[rnd];
            dirs[rnd] = temp;
        }

        foreach (int dir in dirs)
        {
            int nx = x + (dir == 1 ? 1 : (dir == 3 ? -1 : 0));
            int ny = y + (dir == 0 ? 1 : (dir == 2 ? -1 : 0));

            if (nx >= 0 && nx < width && ny >= 0 && ny < height && !visited[nx, ny])
            {
                gridConnections[x, y] |= (1 << dir);
                gridConnections[nx, ny] |= (1 << ((dir + 2) % 4));
                BuildMaze(nx, ny);
            }
        }
    }

    void SpawnTiles()
    {
        gridPipes = new PipePiece[width, height]; 
        float boardWidth = 0.9f;
        float localTileSize = boardWidth / width;
        float startX = -((width - 1) * localTileSize) / 2f;
        float startY = ((height - 1) * localTileSize) / 2f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int mask = gridConnections[x, y];
                GameObject prefabToSpawn;
                int targetRotation;

                DetermineTileType(mask, out prefabToSpawn, out targetRotation);

                if (prefabToSpawn != null)
                {
                    GameObject tileObj = Instantiate(prefabToSpawn, transform);
                    tileObj.transform.localPosition = new Vector3(
                        startX + (x * localTileSize),
                        startY - (y * localTileSize),
                        -0.52f
                    );
                    tileObj.transform.localScale = new Vector3(localTileSize, localTileSize, 1f);

                    PipePiece pipe = tileObj.GetComponent<PipePiece>();
                    if (pipe != null)
                    {
                        pipe.SetRotation(targetRotation);

                        int randomRotations = Random.Range(0, 4);
                        for (int r = 0; r < randomRotations; r++)
                        {
                            pipe.RotatePipe();
                        }

                        gridPipes[x, y] = pipe;
                    }
                }
            }
        }
    }

    void DetermineTileType(int mask, out GameObject prefab, out int rotation)
    {
        int count = 0;
        for (int i = 0; i < 4; i++) if ((mask & (1 << i)) != 0) count++;

        prefab = null;
        rotation = 0;

        if (count == 1) 
        {
            prefab = prefabEnd;
            if ((mask & 1) != 0) rotation = 0;
            else if ((mask & 2) != 0) rotation = 90;
            else if ((mask & 4) != 0) rotation = 180;
            else if ((mask & 8) != 0) rotation = 270;
        }
        else if (count == 2)
        {
            if ((mask & 5) == 5 || (mask & 10) == 10) 
            {
                prefab = prefabStraight;
                rotation = ((mask & 5) == 5) ? 0 : 90;
            }
            else 
            {
                prefab = prefabCorner;
                if ((mask & 3) == 3) rotation = 0;     
                else if ((mask & 6) == 6) rotation = 90;   
                else if ((mask & 12) == 12) rotation = 180;
                else if ((mask & 9) == 9) rotation = 270; 
            }
        }
        else if (count == 3) 
        {
            prefab = prefabT;
            if ((mask & 14) == 14) rotation = 90;   
            else if ((mask & 13) == 13) rotation = 180; 
            else if ((mask & 11) == 11) rotation = 270; 
            else if ((mask & 7) == 7) rotation = 0;   
        }
    }

    public bool CheckWinCondition()
    {
        if (gridPipes == null || isSolved) return false; 

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                PipePiece current = gridPipes[x, y];
                if (current == null) return false;

                bool hasRight = current.HasConnectionInDirection(1);
                if (x < width - 1)
                {
                    bool neighborHasLeft = gridPipes[x + 1, y].HasConnectionInDirection(3);
                    if (hasRight != neighborHasLeft) return false;
                }
                else if (hasRight) return false;

                bool hasBottom = current.HasConnectionInDirection(2);
                if (y < height - 1)
                {
                    bool neighborHasTop = gridPipes[x, y + 1].HasConnectionInDirection(0);
                    if (hasBottom != neighborHasTop) return false;
                }
                else if (hasBottom) return false;

                if (x == 0 && current.HasConnectionInDirection(3)) return false;
                if (y == 0 && current.HasConnectionInDirection(0)) return false;
            }
        }

        OnPuzzleSolved();
        return true;
    }
}