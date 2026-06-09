import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import CreateList from './pages/CreateList';
import Battle from './pages/Battle';
import Results from './pages/Results';

const App = () => (
  <BrowserRouter>
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/create" element={<CreateList />} />
      <Route path="/battle/:id" element={<Battle />} />
      <Route path="/results/:id" element={<Results />} />
    </Routes>
  </BrowserRouter>
);

export default App;
