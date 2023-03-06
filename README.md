# AR_ICP_Matching

해당 프로젝트는 Unity로 제작되었으나, 용량의 문제로 해당 프로젝트를 진행하면서 사용했던 스크립트 위주로 올렸습니다.

## 논문 제목 : AR에서 객체의 증강 위치를 효율적으로 보간하기 위한 새로운 ICP 매칭

AR에서 객체의 증강 위치를 효율적으로 보간하기 위한 새로운 ICP 매칭 알고리즘 제안
<br>진행기간 2020/11/18 - 2022/06/22
<br><img src="https://img.shields.io/badge/Unity-000000?style=for-the-badge&logo=Unity&logoColor=white">
<img src="https://img.shields.io/badge/Firebase-FFCA28?style=for-the-badge&logo=Firebase&logoColor=black">
<img src="https://img.shields.io/badge/Python-3776AB?style=for-the-badge&logo=Python&logoColor=white">

<aside>
📢 2022년 제 66차  한국컴퓨터 정보학회에 제출한 연구이며, 주 저자 입니다. 
Unity AR 상에서 Point Cloud를 효율적으로 매칭하여 증강하기 위한 연구를 진행하였습니다.

</aside>


![image](https://user-images.githubusercontent.com/76572665/223187079-ce789565-c102-41a2-9f22-ed28f4e8bbf5.png)


- **문제 :**
    - 마커 검출 방식은 사전에 마커를 지정할 수 없는 경우에는 사용이 어렵지만, 마커리스는 불안정하여 대부분 평면 검출이나 페이스 검출을 사용합니다.
- **솔루션 :**
    - 본 논문에서는 평면 생성 없이 특징 포인트만을 이용하여 증강하기 위해 현실과의 점 구름을 비교하여 사용자의 위치를 계산한 후 증강합니다.
- **기대효과 :**
    - 모바일뿐만 아니라 도로 공사, 뇌 수술 등 높은 정확도가 필요한 상황에서 사용하여 더욱 안정적으로 사용 가능합니다.

## 🖥 사용 기술 및 라이브러리

- Unity, ARFoundation
- C++(DLL), PCL(Point Cloud Library)
- Python
- Android, NDK

## 🖱 담당한 기능

- 특징 포인트만 사용하여 평면 생성 없이 현실과의 점 구름을 비교한 결과, 실제와 매우 유사한 점 구름을 수집할 수 있습니다. 또한, 이전 알고리즘에 비해 계산량을 줄였습니다.
- 클릭 위치를 기반으로 증강 위치에 가중치를 두어 원하는 곳에 증강을 하고 점 구름 수집 계산 효율을 높였습니다.
- 모바일에서 수집한 GPS 신호와 주변 환경의 특징점으로 구성된 점 구름을 이용하여 ICP 매칭 기법을 통해 효율적인 증강현실 프레임워크를 개발하였으며, 다양한 환경에서도 대부분 안정적인 결과가 나옵니다.
- 실제 환경과 점 구름을 비교하기 위해 Python을 사용하여 비교했습니다.

## 💡 깨달은 점

- 다양한 특징점들을 저장하기 위해, **Firebase를 이용하여 데이터를 처리**
- 유니티에서는 지원하지 않는 특정 라이브러리(PCL)를 사용하기 위해, **DLL로 따로 만들어 쉽게 사용 가능**
- Android에서는 C++ 코드를 실행할 수 없으므로, **NDK를 사용하여 작동**
- 여러 AR 도구가 있었지만, **코드에 쉽게 접근할 수 있는 AR Foundation**을 선택하여 사용하였다.
- 사용 가능한 변수를 활용하기 위해 직접 코드를 해석하여 **코드 해석 능력을 향상**

## 🎈기타

- [https://www.dbpia.co.kr/journal/articleDetail?nodeId=NODE11140491](https://www.dbpia.co.kr/journal/articleDetail?nodeId=NODE11140491)
- 해당 발표 영상은 [여기](https://www.youtube.com/watch?v=abS5pY5J6Hg)를 클릭하면 보실 수 있습니다.
