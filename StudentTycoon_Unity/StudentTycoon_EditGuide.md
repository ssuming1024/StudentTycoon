# 학생 키우기 직접 수정 가이드

이 프로젝트는 직접 고치면서 배우는 프로젝트입니다.
아래 위치를 열면 Codex에게 맡기지 않고도 직접 수정할 수 있습니다.

## 문제 수정하기

수학, 과학, 사회, 프로그래밍, 미술, 음악 문제는 각 문제 씬에서 수정합니다.

1. `Assets/Scenes`에서 수정할 씬을 엽니다.
   - `MathTest`
   - `ScienceTest`
   - `SocietyTest`
   - `ProgrammingTest`
   - `ArtTest`
   - `MusicTest`
2. Hierarchy에서 `OXQuizManager`를 클릭합니다.
3. Inspector에서 `Inspector에서 직접 만들 문제`를 찾습니다.
4. `문제 목록`의 Size를 늘립니다.
5. Element를 펼쳐서 입력합니다.

문제 종류:

- `OX`: O/X 버튼으로 푸는 문제
- `Input`: 입력창에 답을 쓰는 문제

OX 문제:

- `문제 내용`: 화면에 나올 문제
- `OX 정답이 O인가?`: O가 정답이면 체크, X가 정답이면 체크 해제

입력 문제:

- `문제 내용`: 화면에 나올 문제
- `입력 정답`: 정답

복수 정답은 `/`로 나눠서 씁니다.

예시:

```text
선거/투표
8/8월
HTML/html
```

## 국어 타자 문장 수정하기

1. `Assets/Scenes/KoreanTypingTest` 씬을 엽니다.
2. Hierarchy에서 `KoreanTypingGameManager`를 클릭합니다.
3. Inspector에서 `타자 문장 목록`을 수정합니다.

난이도도 Inspector에서 수정할 수 있습니다.

- `성공에 필요한 문장 수`
- `제한 시간`
- `입력창 자동 선택`

## 체육 게임 수정하기

1. `Assets/Scenes/HealthSpaceTest` 씬을 엽니다.
2. Hierarchy에서 `SpaceTapGameManager`를 클릭합니다.
3. Inspector에서 제한 시간, 목표 횟수, 스탯 증가 관련 값을 수정합니다.

## 엔딩 영상 수정하기

프로그래밍 엔딩:

1. `Assets/Scenes/ProgramingEnding` 씬을 엽니다.
2. Hierarchy에서 `RawImage`를 클릭합니다.
3. Inspector의 `VideoPlayer`에서 `Video Clip`을 원하는 영상으로 바꿉니다.

영상이 끝난 뒤 이동할 씬:

1. Hierarchy에서 `EndingLoadStart`를 클릭합니다.
2. Inspector에서 `Next Scene Name`을 수정합니다.

보통은 이렇게 둡니다.

```text
Start
```

## 엔딩 글씨 수정하기

1. `Assets/Scenes/ProgramingEnding` 씬을 엽니다.
2. Hierarchy에서 `Text (TMP)`를 클릭합니다.
3. Inspector의 TextMeshPro 텍스트 내용을 수정합니다.

현재 문구:

```text
프로그래밍 개발자 엔딩
```

## 시작 화면 수정하기

1. `Assets/Scenes/Start` 씬을 엽니다.
2. 시작 화면 이미지는 `StartScreen` 오브젝트에서 수정합니다.
3. 도움말 버튼은 `HelpButton`을 수정합니다.
4. 게임 시작 버튼은 `CustomizeButton` 또는 `StartButton` 관련 오브젝트를 확인합니다.

## 폰트 수정하기

현재 TMP 폰트는 나눔손글씨로 통일되어 있습니다.

사용 중인 폰트:

```text
Assets/TextMesh Pro/Fonts/나눔손글씨 나의 아내 손글씨 SDF.asset
```

새 TMP 텍스트를 만들었는데 글자가 네모로 나오면:

1. 새 Text(TMP) 오브젝트를 클릭합니다.
2. Inspector에서 Font Asset을 위 나눔손글씨 SDF로 바꿉니다.
3. 그래도 안 나오면 Play를 다시 실행해 봅니다.

## 내가 직접 고치기 좋은 순서

1. 문제 문장 하나 바꾸기
2. 정답 하나 바꾸기
3. 복수정답 추가하기
4. 국어 타자 문장 바꾸기
5. 엔딩 문구 바꾸기
6. 제한 시간이나 목표 개수 바꾸기

작게 바꾸고 바로 Play로 확인하는 방식이 가장 좋습니다.
