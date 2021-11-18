import React, { useEffect, useState } from "react";
import "./Form.css"

function App() {
  const [ userOptions, setUserOptions ] = useState([]);
  const [ selectedUserOptions, setSelectedUserOptions ] = useState(1);
  const [ selectedRecommendationOptions, setSelectedRecommendationOptions ] = useState([]);
  const [ recommendationOptions, setRecommendationOptions ] = useState([]);
  const [ algoritmOptions, setAlgoritmOptions ] = useState("ubcf/ed");
  const [ kOptions, setKOptions ] = useState(3);


  useEffect(() => {
    async function fetchData() {
      let users = await fetch("https://localhost:5001/user");
      setUserOptions(await users.json());
    }
    fetchData();
  }, []);



  useEffect(() => {
    const renderTableBody = () => {
      return recommendationOptions.map(recommendation => {
        const {movieId, movieTitle, averageWeightedRating} = recommendation;
        return (
        <tr key={movieId}>
          <td>{movieTitle}</td>
          <td>{movieId}</td>
          <td>{averageWeightedRating.toFixed(2)}</td>
        </tr>
        )
      })}

    setSelectedRecommendationOptions(renderTableBody)
  }, [recommendationOptions]);

  const handleAlgoritmChange = (e) => {
    setAlgoritmOptions(e.target.value);
  };

  const handleUserChange = (e) => {
    setSelectedUserOptions(e.target.value);
  };

  const handleKChange = (e) => {
    setKOptions(e.target.value);
  };

  const recommendMovies = async (e) => {
    e.preventDefault();
    let recommendations = await fetch(`https://localhost:5001/recommendation/${algoritmOptions}?userid=${selectedUserOptions}&k=${kOptions}`);
    setRecommendationOptions(await recommendations.json());
  };

  return (
    <div>
      <header>
        <h1>Recommendation System</h1>
      </header>
      <form className = "form">
        <div>
          <label>User:</label>
            <select onChange={handleUserChange}>
              {userOptions.map(user => (
                <option key={user.userId} value={user.userId}>
                  {user.userId}: {user.userName}
                </option>
              ))}
            </select>
        </div>

        <div>
          <label>Method:</label>
            <select onChange={handleAlgoritmChange}>
              <option key="euclidian" value="ubcf/ed">User-Based (Euclidian Distance)</option>
              <option key="pearsson" value="ubcf/pc">User-Based (Pearsson Correlation)</option>
              <option key="itembased" value="ibcf/ed">Item-Based (Euclidian Distance)</option>
            </select>
        </div>

        <div>
          <label>Result:</label>
            <input type="number" min={1} max={10} value={kOptions} onChange={handleKChange}>
            </input>

        </div>

        <div>
          <button className ="submitButton" onClick={recommendMovies}>
            Find recommended movies
          </button>
        </div>
      </form>

      <div className = "form">
        <table>
          <thead>
            <tr>
              <th>Name</th>
              <th>Id</th>
              <th>Score</th>
            </tr>
          </thead>
          <tbody>
            {selectedRecommendationOptions}
          </tbody>

         </table>
      </div>

    </div>
  );
}

export default App;
