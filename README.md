# NES-Emu
Cycle accurate Nintendo NES emulator in C#.\
![smb](https://user-images.githubusercontent.com/38964466/43566223-bbb466bc-95fb-11e8-9313-d0652166a422.png)\
![debugger](https://user-images.githubusercontent.com/38964466/43566227-bfa2ee1a-95fb-11e8-886f-d1fc1ed24802.png)

# Progress
CPU: All official instructions implemented and tested. Runs a single cycle each game loop.  

PPU: 95%, Some edge case timing issues with sprite overflow and sprite zero hits.

APU: Pulse1, Pulse2, Triangle and Noise channels are implemented.

Mappers: 0,1,2,3,4,7,9,11,13,34,66,71,232

Inputs: 1 player either keyboard or Xinput(Xbox) gamepad, configurable through a gui.

Debugger: Supports adding breakpoints, Memory search, Hex converter, Dynamic disassembler,\
Image testing tools, PPU viewer and log comparison tool with error analyzer.

# Controls

Default keyboard setup

| NES           | Keyboard      |
| ------------- | ------------- |
| Up            | W             |
| Down          | S             |
| Left          | A             |
| Right         | D             |
| Select        | U             |
| Start         | I             |
| A             | N             |
| B             | M             |

# Screenshots

![castlevania u prg1 -256](https://user-images.githubusercontent.com/38964466/43570249-daba566e-9607-11e8-9a72-22dec66346a4.png)
![duck tales u -256](https://user-images.githubusercontent.com/38964466/43570255-dc2e0658-9607-11e8-822d-69dc3644c047.png)
![baseball u -256](https://user-images.githubusercontent.com/38964466/43570258-dd698290-9607-11e8-974c-cd9bdbbe8a1a.png)
![dragon warrior iv u -256](https://user-images.githubusercontent.com/38964466/43570263-e0e7b3ce-9607-11e8-898f-688fb617e827.png)
![advanced dungeons dragons - hillsfar u -256](https://user-images.githubusercontent.com/38964466/43570268-e4634450-9607-11e8-9369-58c2446c89f7.png)
![adventures of tom sawyer u -256](https://user-images.githubusercontent.com/38964466/43570271-e57bb714-9607-11e8-8192-c047f823fcaa.png)
![bases loaded u prg2 -256](https://user-images.githubusercontent.com/38964466/43570275-e7f01332-9607-11e8-8a36-eb37dda4b900.png)
![boy and his blob a - trouble on blobolonia u -256](https://user-images.githubusercontent.com/38964466/43570279-e8ccc034-9607-11e8-88d5-fb2fcf21ac9a.png)
![castlevania ii - simon s quest u -256](https://user-images.githubusercontent.com/38964466/43570287-eb3a1c7c-9607-11e8-9500-cca7f694b7ad.png)
![dr mario u prg1 -256](https://user-images.githubusercontent.com/38964466/43570288-ec1ca218-9607-11e8-96ae-ef3c9567048c.png)
![monopoly u -256](https://user-images.githubusercontent.com/38964466/43570300-f4ebb654-9607-11e8-9c65-66362da2a92a.png)
![nes open tournament golf u -256](https://user-images.githubusercontent.com/38964466/43570306-f756fa7a-9607-11e8-8aa4-c00474ed0379.png)
![simpsons the - bart vs the space mutants u prg0 -256](https://user-images.githubusercontent.com/38964466/43570307-f8518030-9607-11e8-9f52-5c2bed281290.png)
![snow bros u p -256](https://user-images.githubusercontent.com/38964466/43570312-fa6e0fd2-9607-11e8-8c16-588f2068ed87.png)
![super dodge ball u -256](https://user-images.githubusercontent.com/38964466/43570313-fb48c0c8-9607-11e8-8222-008ddeca8601.png)
![yoshi u -256](https://user-images.githubusercontent.com/38964466/43570316-fd69062e-9607-11e8-9608-e83f8745e45e.png)
![uncanny x-men the u -256](https://user-images.githubusercontent.com/38964466/43570320-00e2521a-9608-11e8-98b7-134e471f930f.png)
![super mario bros 2 u prg1 -256](https://user-images.githubusercontent.com/38964466/43570322-02c298ba-9608-11e8-9854-e86c06386206.png)
![star trek - 25th anniversary u -256](https://user-images.githubusercontent.com/38964466/43570324-038ed13c-9608-11e8-83b0-86b4a3dff8b8.png)
![gauntlet u -256](https://user-images.githubusercontent.com/38964466/43570326-063d0570-9608-11e8-9297-7ecb2d204178.png)
![advanced dungeons dragons - dragon strike u -256](https://user-images.githubusercontent.com/38964466/43570329-074c5a42-9608-11e8-943b-8ced8d004057.png)
