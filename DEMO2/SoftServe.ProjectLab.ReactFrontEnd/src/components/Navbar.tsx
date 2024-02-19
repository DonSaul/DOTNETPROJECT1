const Navbar = () => {
  return (
    <nav
      style={{
        position: "absolute",
        width: "30%",
        top: "3%",
        left: "35%",
        right: "35%",
      }}
    >
      <a href="technicians">Technicians</a>
      <a href="workOrders">Work Orders</a>
    </nav>
  );
};

export default Navbar;
